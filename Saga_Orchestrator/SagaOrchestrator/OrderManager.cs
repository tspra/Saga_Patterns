using SagaOrchestrator.Inventory;
using SagaOrchestrator.Notifier;
using SagaOrchestrator.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagaOrchestrator
{
    public enum OrderTransactionState
    {
        NotStarted,
        OrderCreated,
        OrderCancelled,
        OrderCreateFailed,
        InventoryUpdated,
        InventoryUpdateFailed,
        InventoryRollback,
        NotificationSend,
        NotificationFailed
    }
    public enum OrderAction
    {
        CreateOrder,
        CancelOrder,
        UpdateInventory,
        RollbackInventory,
        SendNotification
    }
    public class OrderManager : IOrderManager
    {
        private readonly IOrderProxy orderProxy;
        private readonly INotifierProxy notifierProxy;
        private readonly IInventoryProxy inventoryProxy;
        public OrderManager(IOrderProxy orProxy, IInventoryProxy invProxy, INotifierProxy notProxy)
        {
            orderProxy = orProxy;
            inventoryProxy = invProxy;
            notifierProxy = notProxy;
        }

        public bool CreateOrder(OrderItem order)
        {
            var orderStateMachine = new Stateless.StateMachine<OrderTransactionState, OrderAction>(OrderTransactionState.NotStarted);
            int orderId = -1;
            bool isOrderSucess = false;

            orderStateMachine.Configure(OrderTransactionState.NotStarted)
                .PermitDynamic(OrderAction.CreateOrder, () =>
                 {
                     (orderId, isOrderSucess) = orderProxy.CreateOrder(order).Result;
                     return isOrderSucess ? OrderTransactionState.OrderCreated : OrderTransactionState.OrderCreateFailed;
                 });

            orderStateMachine.Configure(OrderTransactionState.OrderCreated)
              .PermitDynamic(OrderAction.UpdateInventory, () =>
              {
                  var (InventoryId, isSucess) = inventoryProxy.UpdateInventory(order).Result;
                  return isSucess ? OrderTransactionState.InventoryUpdated : OrderTransactionState.InventoryUpdateFailed;
              }).OnEntry(() =>orderStateMachine.Fire(OrderAction.UpdateInventory));

            orderStateMachine.Configure(OrderTransactionState.InventoryUpdated)
             .PermitDynamic(OrderAction.SendNotification, () =>
             {
                 var (notificationId, isSucess) = notifierProxy.Send(order).Result;
                 return isSucess ? OrderTransactionState.NotificationSend : OrderTransactionState.NotificationFailed;
             }) .OnEntry(() => orderStateMachine.Fire(OrderAction.SendNotification));

            orderStateMachine.Configure(OrderTransactionState.InventoryUpdateFailed)
            .PermitDynamic(OrderAction.RollbackInventory, () =>
            {
                inventoryProxy.Delete(orderId);
                return OrderTransactionState.InventoryRollback;
            }).OnEntry(() => orderStateMachine.Fire(OrderAction.RollbackInventory));


            orderStateMachine.Configure(OrderTransactionState.InventoryRollback)
          .PermitDynamic(OrderAction.CancelOrder, () =>
          {
              orderProxy.DeleteOrder(orderId);
              return OrderTransactionState.OrderCancelled;
          }).OnEntry(() => orderStateMachine.Fire(OrderAction.CancelOrder));

            orderStateMachine.Fire(OrderAction.CreateOrder);

            return orderStateMachine.State == OrderTransactionState.NotificationSend;
        }
    }
}
