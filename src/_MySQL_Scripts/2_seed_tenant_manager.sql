-- Adminer 4.8.1 MySQL 8.0.26 dump

SET NAMES utf8;
SET time_zone = '+00:00';
SET foreign_key_checks = 0;
SET sql_mode = 'NO_AUTO_VALUE_ON_ZERO';

SET NAMES utf8mb4;

DROP DATABASE IF EXISTS `debug:tenant_manager`;
CREATE DATABASE `debug:tenant_manager` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci */ ;
USE `debug:tenant_manager`;

DROP TABLE IF EXISTS `__EFMigrationsHistory`;
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

TRUNCATE `__EFMigrationsHistory`;
INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`) VALUES
('20210825144318_init',	'5.0.8');

DROP TABLE IF EXISTS `tenant_manager_customization`;
CREATE TABLE `tenant_manager_customization` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ControllerName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `MethodName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `ServiceName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `IsActive` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

# TRUNCATE `tenant_manager_customization`;
# INSERT INTO `tenant_manager_customization` (`Id`, `ControllerName`, `MethodName`, `ServiceName`, `IsActive`) VALUES
# (1,	'TestTenantCustomization',	'Index',	'debug:test',	1);

DROP DATABASE IF EXISTS `one:tenant_manager`;
CREATE DATABASE `one:tenant_manager` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci */ ;
USE `one:tenant_manager`;

DROP TABLE IF EXISTS `__EFMigrationsHistory`;
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

TRUNCATE `__EFMigrationsHistory`;
INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`) VALUES
('20210825144318_init',	'5.0.8');

DROP TABLE IF EXISTS `tenant_manager_customization`;
CREATE TABLE `tenant_manager_customization` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ControllerName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `MethodName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `ServiceName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `IsActive` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

TRUNCATE `tenant_manager_customization`;
INSERT INTO `tenant_manager_customization` (`Id`, `ControllerName`, `MethodName`, `ServiceName`, `IsActive`) VALUES
(1,	'Catalog',	'Index',	'one:CatalogCustomization',	1);

DROP DATABASE IF EXISTS `two:tenant_manager`;
CREATE DATABASE `two:tenant_manager` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci */ ;
USE `two:tenant_manager`;

DROP TABLE IF EXISTS `__EFMigrationsHistory`;
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

TRUNCATE `__EFMigrationsHistory`;
INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`) VALUES
('20210825144318_init',	'5.0.8');

DROP TABLE IF EXISTS `tenant_manager_customization`;
CREATE TABLE `tenant_manager_customization` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ControllerName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `MethodName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `ServiceName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `IsActive` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

# TRUNCATE `tenant_manager_customization`;
# INSERT INTO `tenant_manager_customization` (`Id`, `ControllerName`, `MethodName`, `ServiceName`, `IsActive`) VALUES
# (1,	'TestTenantCustomization',	'Index',	'one:test',	1);

-- 2021-08-25 16:49:49
