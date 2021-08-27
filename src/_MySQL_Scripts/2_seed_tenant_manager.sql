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
  `ControllerName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `MethodName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ServiceName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ServiceEndPoint` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `IsActive` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

TRUNCATE `tenant_manager_customization`;
INSERT INTO `tenant_manager_customization` (`Id`, `ControllerName`, `MethodName`, `ServiceName`, `ServiceEndPoint`, `IsActive`) VALUES
(1,	'TestTenantCustomization',	'Index',	'debug:test',	'/TestTenantCustomization',	1);

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
  `ControllerName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `MethodName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ServiceName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ServiceEndPoint` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `IsActive` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

TRUNCATE `tenant_manager_customization`;
INSERT INTO `tenant_manager_customization` (`Id`, `ControllerName`, `MethodName`, `ServiceName`, `ServiceEndPoint`, `IsActive`) VALUES
(1,	'Catalog',	'Index',	'one:CatalogCustomization',	'/catalog',	1),
(2,	'Catalog',	'ViewItem',	'one:CatalogCustomization',	'/catalog/{id}',1),
(3,	'Cart',	'Index',	'one:CatalogCustomization',	'/cart',0),
(4,	'Cart',	'UpdateCart',	'one:CatalogCustomization',	'/cart/update',0);

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
  `ControllerName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `MethodName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ServiceName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ServiceEndPoint` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `IsActive` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- 2021-08-26 12:19:29
