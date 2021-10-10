-- Adminer 4.8.1 MySQL 8.0.26 dump

SET NAMES utf8;
SET time_zone = '+00:00';
SET foreign_key_checks = 0;
SET sql_mode = 'NO_AUTO_VALUE_ON_ZERO';

SET NAMES utf8mb4;

DROP DATABASE IF EXISTS `debug:tenant_manager`;
CREATE DATABASE `debug:tenant_manager` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `debug:tenant_manager`;

DROP TABLE IF EXISTS `__EFMigrationsHistory`;
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

TRUNCATE `__EFMigrationsHistory`;
INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`) VALUES
('20210826121712_init',	'5.0.8');

DROP TABLE IF EXISTS `tenant_manager_customization`;
CREATE TABLE `tenant_manager_customization` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Title` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ControllerName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `MethodName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ServiceName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ServiceEndPoint` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `IsActive` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

TRUNCATE `tenant_manager_customization`;

DROP TABLE IF EXISTS `tenant_manager_customization_point`;
CREATE TABLE `tenant_manager_customization_point` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ControllerName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `MethodName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `CodeSnippet` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

TRUNCATE `tenant_manager_customization_point`;
INSERT INTO `tenant_manager_customization_point` (`Id`, `Description`, `ControllerName`, `MethodName`, `CodeSnippet`) VALUES (1,'Load catalog view for the tenant','Catalog','Index','public async Task<IActionResult> Index()\n{\n    // Get Catalog items from CatalogService\n    var items = await _catalogService.GetCatalogItems();\n    // create view model\n    var data = new IndexViewModel()\n    {\n        CatalogItems = items\n    };\n    // load viwe\n    return View(data);\n}'),(2,'Get the cart for the user','Cart','Index','public async Task<IActionResult> Index()\n{\n    // get the user id \n    var user = _identityParser.Parse(HttpContext.User);\n    // get the cart of user\n    var cartDetails = await _cartService.GetCart(user);\n    // load view\n    return View(cartDetails);\n}'),(3,'Add item to user\'s cart','Cart','AddItemToCart','public async Task<IActionResult> AddItemToCart(int id, int qty)\n{\n    // TODO: ApiGateway Agregation\n    // STEP 1: Get item\n    var item = await _catalogService.GetSingleCatalogItemById(id);\n    if(item?.Id != null)\n    {\n        // get the user id \n        var user = _identityParser.Parse(HttpContext.User);\n        var newCartItem = new CartItemDTO\n        {\n            ItemId = item.Id,\n            Description = item.Description,\n            Name = item.Name,\n            Price = item.Price,\n            Quantity = qty\n        };\n        // STEP 2: Add item to cart\n        await _cartService.AddItemToCart(user, newCartItem);\n    }\n\n    return RedirectToAction(\"Index\", \"Cart\");\n}'),(4,'Update user\'s cart','Cart','UpdateCart','public async Task<IActionResult> UpdateCart(UpdateCartItemDTO[] items)\n{\n    if(items.Length == 0)\n    {\n        return RedirectToAction(\"Index\", \"Cart\");\n    }\n    // get the user id \n    var user = _identityParser.Parse(HttpContext.User);\n    var cartDetails = await _cartService.GetCart(user);\n\n    foreach (UpdateCartItemDTO itm in items)\n    {\n        var cartItem = cartDetails.Items.FirstOrDefault(i => i.Id == itm.Id);\n        cartItem.Quantity = itm.Quantity;\n    }\n\n    await _cartService.UpdateCart(user, cartDetails);\n\n    return RedirectToAction(\"Index\", \"Cart\");\n}'),(5,'Get orders for the user','Order','Index','public async Task<IActionResult> Index()\n{\n    var orders = await _orderService.GetOrdersForUser();\n    var data = new OrdersViewModel\n    {\n        Orders = orders\n    };\n    return View(data);\n}'),(6,'Create order for user','Order','CreateOrder','public async Task<IActionResult> CreateOrder()\n{\n    // TODO: Add to aggregator\n    // STEP 1: Get Cart info\n    var user = _identityParser.Parse(HttpContext.User);\n    CartDetails cart = await _cartService.GetCart(user);\n\n    if(cart != null)\n    {\n        // STEP 2: Create order\n        var order = new OrderDTO\n        {\n            TotalPaid = cart.Total\n        };\n\n        foreach(CartItem item in cart.Items)\n        {\n            var newOrderItem = new OrderItemDTO\n            {\n                ItemId = item.Id,\n                Name = item.Name,\n                Description = item.Description,\n                Price = item.Price,\n                Quantity = item.Quantity\n            };\n\n            order.Items.Add(newOrderItem);\n        }\n\n        var orderCreated = await _orderService.CreateOrder(order);\n\n        if(orderCreated == false)\n        {\n            return RedirectToAction(\"Index\", \"Cart\");\n        }\n\n        //STEP 3: Delete Cart\n        // this should be done throuhg Event Bus\n        await _cartService.DeleteCart(user);\n\n        return RedirectToAction(\"Index\", \"Order\");\n\n    }\n    return RedirectToAction(\"Index\", \"Cart\");\n}');



DROP DATABASE IF EXISTS `one:tenant_manager`;
CREATE DATABASE `one:tenant_manager` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `one:tenant_manager`;

DROP TABLE IF EXISTS `__EFMigrationsHistory`;
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

TRUNCATE `__EFMigrationsHistory`;
INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`) VALUES
('20210826121712_init',	'5.0.8');

DROP TABLE IF EXISTS `tenant_manager_customization`;
CREATE TABLE `tenant_manager_customization` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Title` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ControllerName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `MethodName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ServiceName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ServiceEndPoint` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `IsActive` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

TRUNCATE `tenant_manager_customization`;
INSERT INTO `tenant_manager_customization` (`Id`, `Title`, `Description`, `ControllerName`, `MethodName`, `ServiceName`, `ServiceEndPoint`, `IsActive`) VALUES
(1,	'Custom Catalog Page', '', 'Catalog',	'Index',	'CatalogCustomization',	'/catalog',	1),
(2,	'View Catalog Item Page', '', 'Catalog',	'ViewItem',	'CatalogCustomization',	'/catalog/{id}',1),
(3,	'Custom Cart Page', '', 'Cart',	'Index',	'CatalogCustomization',	'/cart',1),
(4,	'Custom Update Cart Functionality', '', 'Cart',	'UpdateCart',	'CatalogCustomization',	'/cart/update',1);

DROP TABLE IF EXISTS `tenant_manager_customization_point`;
CREATE TABLE `tenant_manager_customization_point` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ControllerName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `MethodName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `CodeSnippet` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

TRUNCATE `tenant_manager_customization_point`;
INSERT INTO `tenant_manager_customization_point` (`Id`, `Description`, `ControllerName`, `MethodName`, `CodeSnippet`) VALUES (1,'Load catalog view for the tenant','Catalog','Index','public async Task<IActionResult> Index()\n{\n    // Get Catalog items from CatalogService\n    var items = await _catalogService.GetCatalogItems();\n    // create view model\n    var data = new IndexViewModel()\n    {\n        CatalogItems = items\n    };\n    // load viwe\n    return View(data);\n}'),(2,'Get the cart for the user','Cart','Index','public async Task<IActionResult> Index()\n{\n    // get the user id \n    var user = _identityParser.Parse(HttpContext.User);\n    // get the cart of user\n    var cartDetails = await _cartService.GetCart(user);\n    // load view\n    return View(cartDetails);\n}'),(3,'Add item to user\'s cart','Cart','AddItemToCart','public async Task<IActionResult> AddItemToCart(int id, int qty)\n{\n    // TODO: ApiGateway Agregation\n    // STEP 1: Get item\n    var item = await _catalogService.GetSingleCatalogItemById(id);\n    if(item?.Id != null)\n    {\n        // get the user id \n        var user = _identityParser.Parse(HttpContext.User);\n        var newCartItem = new CartItemDTO\n        {\n            ItemId = item.Id,\n            Description = item.Description,\n            Name = item.Name,\n            Price = item.Price,\n            Quantity = qty\n        };\n        // STEP 2: Add item to cart\n        await _cartService.AddItemToCart(user, newCartItem);\n    }\n\n    return RedirectToAction(\"Index\", \"Cart\");\n}'),(4,'Update user\'s cart','Cart','UpdateCart','public async Task<IActionResult> UpdateCart(UpdateCartItemDTO[] items)\n{\n    if(items.Length == 0)\n    {\n        return RedirectToAction(\"Index\", \"Cart\");\n    }\n    // get the user id \n    var user = _identityParser.Parse(HttpContext.User);\n    var cartDetails = await _cartService.GetCart(user);\n\n    foreach (UpdateCartItemDTO itm in items)\n    {\n        var cartItem = cartDetails.Items.FirstOrDefault(i => i.Id == itm.Id);\n        cartItem.Quantity = itm.Quantity;\n    }\n\n    await _cartService.UpdateCart(user, cartDetails);\n\n    return RedirectToAction(\"Index\", \"Cart\");\n}'),(5,'Get orders for the user','Order','Index','public async Task<IActionResult> Index()\n{\n    var orders = await _orderService.GetOrdersForUser();\n    var data = new OrdersViewModel\n    {\n        Orders = orders\n    };\n    return View(data);\n}'),(6,'Create order for user','Order','CreateOrder','public async Task<IActionResult> CreateOrder()\n{\n    // TODO: Add to aggregator\n    // STEP 1: Get Cart info\n    var user = _identityParser.Parse(HttpContext.User);\n    CartDetails cart = await _cartService.GetCart(user);\n\n    if(cart != null)\n    {\n        // STEP 2: Create order\n        var order = new OrderDTO\n        {\n            TotalPaid = cart.Total\n        };\n\n        foreach(CartItem item in cart.Items)\n        {\n            var newOrderItem = new OrderItemDTO\n            {\n                ItemId = item.Id,\n                Name = item.Name,\n                Description = item.Description,\n                Price = item.Price,\n                Quantity = item.Quantity\n            };\n\n            order.Items.Add(newOrderItem);\n        }\n\n        var orderCreated = await _orderService.CreateOrder(order);\n\n        if(orderCreated == false)\n        {\n            return RedirectToAction(\"Index\", \"Cart\");\n        }\n\n        //STEP 3: Delete Cart\n        // this should be done throuhg Event Bus\n        await _cartService.DeleteCart(user);\n\n        return RedirectToAction(\"Index\", \"Order\");\n\n    }\n    return RedirectToAction(\"Index\", \"Cart\");\n}');


DROP DATABASE IF EXISTS `two:tenant_manager`;
CREATE DATABASE `two:tenant_manager` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `two:tenant_manager`;

DROP TABLE IF EXISTS `__EFMigrationsHistory`;
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

TRUNCATE `__EFMigrationsHistory`;
INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`) VALUES
('20210826121712_init',	'5.0.8');

DROP TABLE IF EXISTS `tenant_manager_customization`;
CREATE TABLE `tenant_manager_customization` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Title` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ControllerName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `MethodName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ServiceName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ServiceEndPoint` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `IsActive` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

DROP TABLE IF EXISTS `tenant_manager_customization_point`;
CREATE TABLE `tenant_manager_customization_point` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ControllerName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `MethodName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `CodeSnippet` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

TRUNCATE `tenant_manager_customization_point`;
INSERT INTO `tenant_manager_customization_point` (`Id`, `Description`, `ControllerName`, `MethodName`, `CodeSnippet`) VALUES (1,'Load catalog view for the tenant','Catalog','Index','public async Task<IActionResult> Index()\n{\n    // Get Catalog items from CatalogService\n    var items = await _catalogService.GetCatalogItems();\n    // create view model\n    var data = new IndexViewModel()\n    {\n        CatalogItems = items\n    };\n    // load viwe\n    return View(data);\n}'),(2,'Get the cart for the user','Cart','Index','public async Task<IActionResult> Index()\n{\n    // get the user id \n    var user = _identityParser.Parse(HttpContext.User);\n    // get the cart of user\n    var cartDetails = await _cartService.GetCart(user);\n    // load view\n    return View(cartDetails);\n}'),(3,'Add item to user\'s cart','Cart','AddItemToCart','public async Task<IActionResult> AddItemToCart(int id, int qty)\n{\n    // TODO: ApiGateway Agregation\n    // STEP 1: Get item\n    var item = await _catalogService.GetSingleCatalogItemById(id);\n    if(item?.Id != null)\n    {\n        // get the user id \n        var user = _identityParser.Parse(HttpContext.User);\n        var newCartItem = new CartItemDTO\n        {\n            ItemId = item.Id,\n            Description = item.Description,\n            Name = item.Name,\n            Price = item.Price,\n            Quantity = qty\n        };\n        // STEP 2: Add item to cart\n        await _cartService.AddItemToCart(user, newCartItem);\n    }\n\n    return RedirectToAction(\"Index\", \"Cart\");\n}'),(4,'Update user\'s cart','Cart','UpdateCart','public async Task<IActionResult> UpdateCart(UpdateCartItemDTO[] items)\n{\n    if(items.Length == 0)\n    {\n        return RedirectToAction(\"Index\", \"Cart\");\n    }\n    // get the user id \n    var user = _identityParser.Parse(HttpContext.User);\n    var cartDetails = await _cartService.GetCart(user);\n\n    foreach (UpdateCartItemDTO itm in items)\n    {\n        var cartItem = cartDetails.Items.FirstOrDefault(i => i.Id == itm.Id);\n        cartItem.Quantity = itm.Quantity;\n    }\n\n    await _cartService.UpdateCart(user, cartDetails);\n\n    return RedirectToAction(\"Index\", \"Cart\");\n}'),(5,'Get orders for the user','Order','Index','public async Task<IActionResult> Index()\n{\n    var orders = await _orderService.GetOrdersForUser();\n    var data = new OrdersViewModel\n    {\n        Orders = orders\n    };\n    return View(data);\n}'),(6,'Create order for user','Order','CreateOrder','public async Task<IActionResult> CreateOrder()\n{\n    // TODO: Add to aggregator\n    // STEP 1: Get Cart info\n    var user = _identityParser.Parse(HttpContext.User);\n    CartDetails cart = await _cartService.GetCart(user);\n\n    if(cart != null)\n    {\n        // STEP 2: Create order\n        var order = new OrderDTO\n        {\n            TotalPaid = cart.Total\n        };\n\n        foreach(CartItem item in cart.Items)\n        {\n            var newOrderItem = new OrderItemDTO\n            {\n                ItemId = item.Id,\n                Name = item.Name,\n                Description = item.Description,\n                Price = item.Price,\n                Quantity = item.Quantity\n            };\n\n            order.Items.Add(newOrderItem);\n        }\n\n        var orderCreated = await _orderService.CreateOrder(order);\n\n        if(orderCreated == false)\n        {\n            return RedirectToAction(\"Index\", \"Cart\");\n        }\n\n        //STEP 3: Delete Cart\n        // this should be done throuhg Event Bus\n        await _cartService.DeleteCart(user);\n\n        return RedirectToAction(\"Index\", \"Order\");\n\n    }\n    return RedirectToAction(\"Index\", \"Cart\");\n}');

-- 2021-08-26 12:19:29
