CREATE TABLE [FoodOrder] (
    [FoodsId] bigint NOT NULL,
    [OrdersId] bigint NOT NULL,
    CONSTRAINT [PK_FoodOrder] PRIMARY KEY ([FoodsId], [OrdersId]),
    CONSTRAINT [FK_FoodOrder_Foods_FoodsId] FOREIGN KEY ([FoodsId]) REFERENCES [Foods] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_FoodOrder_Orders_OrdersId] FOREIGN KEY ([OrdersId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE
);
GO
CREATE INDEX [IX_FoodOrder_OrdersId] ON [FoodOrder] ([OrdersId]);