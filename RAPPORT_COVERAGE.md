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
| ProductService.cs / GetProductByIdAsync | GetProductByIdAsync_Should_ReturnProduct_When_Exists | Retourne le produit si trouvé |
| ProductService.cs / GetProductByIdAsync | GetProductByIdAsync_Should_ReturnNull_When_NotExists | Retourne null si pas trouvé |
| ProductService.cs / GetAllProductsAsync | GetAllProductsAsync_Should_ReturnList | Retourne la liste des produits |
| ProductService.cs / CreateProductAsync | CreateProductAsync_Should_CallRepository | Vérifie l'appel au repository |
| ProductsController.cs / GetAll | GetAll_Should_ReturnOk_WithList | 200 OK avec liste |
| ProductsController.cs / GetById | GetById_Should_ReturnOk_WhenFound | 200 OK avec produit |
| ProductsController.cs / GetById | GetById_Should_ReturnNotFound_WhenNotFound | 404 Not Found si inconnu |
| ProductsController.cs / Create | Create_Should_ReturnCreated | 201 Created |
| AuthService.cs / LoginAsync | LoginAsync_Should_ReturnToken_When_CredentialsValid | Retourne un token si succès |
| AuthService.cs / LoginAsync | LoginAsync_Should_ReturnNull_When_UserNotFound | Retourne null si user inexistant |
| AuthService.cs / LoginAsync | LoginAsync_Should_ReturnNull_When_PasswordInvalid | Retourne null si mot de passe incorrect |
