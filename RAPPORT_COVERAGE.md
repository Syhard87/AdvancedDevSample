# RAPPORT DE COUVERTURE (COVERAGE REPORT)

| Fichier / Méthode Non Couvert | Test Ajouté (Nom) | Cas Spécifique Couvert |
|-------------------------------|-------------------|------------------------|
| Order.cs / AddOrderItem | AddOrderItem_Should_AddItem_When_Valid | Ajout d'un item valide et calcul du total |
| Order.cs / AddOrderItem | AddOrderItem_Should_UpdateQuantity_When_ItemExists | Mise à jour de la quantité si l'item existe |
| Order.cs / AddOrderItem | AddOrderItem_Should_ThrowArgumentException_When_QuantityIsZeroOrNegative | Exception si quantité <= 0 |
| Order.cs / AddOrderItem | AddOrderItem_Should_ThrowArgumentException_When_UnitPriceIsNegative | Exception si prix unitaire < 0 |
| OrderService.cs / CreateOrderAsync | CreateOrderAsync_ShouldThrowArgumentException_When_UserIdIsEmpty | Exception si UserId est vide |
| OrderService.cs / CreateOrderAsync | CreateOrderAsync_ShouldThrowArgumentException_When_ItemsIsEmpty | Exception si liste items vide/null |
| OrderService.cs / CreateOrderAsync | CreateOrderAsync_Should_LogWarning_When_ProductNotFound | Vérification du log warning |
| OrdersController.cs / CreateOrder | CreateOrder_Should_ReturnUnauthorized_When_UserClaimMissing | 401 Unauthorized si token invalide |
| OrdersController.cs / CreateOrder | CreateOrder_Should_ReturnBadRequest_When_UserIdInvalid | 400 Bad Request si User ID invalide |
| OrdersController.cs / CreateOrder | CreateOrder_Should_ReturnCreated_When_Success | 201 Created avec réponse |
| OrdersController.cs / CreateOrder | CreateOrder_Should_ReturnNotFound_When_ProductNotFound | 404 Not Found si produit inconnu |
