-- MySQL dump 10.13  Distrib 8.0.40, for Win64 (x86_64)
--
-- Host: localhost    Database: toemall
-- ------------------------------------------------------
-- Server version	8.0.40

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `carts`
--

DROP TABLE IF EXISTS `carts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `carts` (
  `CartId` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `ProductId` int NOT NULL,
  `Quantity` int DEFAULT '1',
  `CreatedAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`CartId`),
  KEY `UserId` (`UserId`),
  KEY `ProductId` (`ProductId`),
  CONSTRAINT `carts_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`) ON DELETE CASCADE,
  CONSTRAINT `carts_ibfk_2` FOREIGN KEY (`ProductId`) REFERENCES `products` (`ProductId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `carts`
--

LOCK TABLES `carts` WRITE;
/*!40000 ALTER TABLE `carts` DISABLE KEYS */;
/*!40000 ALTER TABLE `carts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `categories`
--

DROP TABLE IF EXISTS `categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categories` (
  `CategoryId` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  `Description` text,
  PRIMARY KEY (`CategoryId`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categories`
--

LOCK TABLES `categories` WRITE;
/*!40000 ALTER TABLE `categories` DISABLE KEYS */;
INSERT INTO `categories` VALUES (1,'电器','生活电器'),(2,'手机','手机'),(3,'日用','日用百货'),(4,'速食','速食'),(5,'生鲜','生鲜'),(6,'水果','水果'),(7,'电脑','电脑'),(8,'衣物','时尚穿搭'),(9,'零食','零食'),(10,'家具','家具'),(11,'学习','学习');
/*!40000 ALTER TABLE `categories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `orders`
--

DROP TABLE IF EXISTS `orders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `orders` (
  `OrderId` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `ProductId` int NOT NULL,
  `ProductName` varchar(100) NOT NULL,
  `Quantity` int NOT NULL DEFAULT '1',
  `Price` decimal(10,2) DEFAULT NULL,
  `TotalPrice` decimal(10,2) DEFAULT NULL,
  `OrderStatus` enum('Unpaid','Paid','Shipping','Cancelled') DEFAULT 'Unpaid',
  `CreatedAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `UpdatedAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`OrderId`),
  KEY `UserId` (`UserId`),
  KEY `ProductId` (`ProductId`),
  CONSTRAINT `orders_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`) ON DELETE CASCADE,
  CONSTRAINT `orders_ibfk_2` FOREIGN KEY (`ProductId`) REFERENCES `products` (`ProductId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orders`
--

LOCK TABLES `orders` WRITE;
/*!40000 ALTER TABLE `orders` DISABLE KEYS */;
INSERT INTO `orders` VALUES (1,1,1,'iPhone 15 Pro',1,999.99,999.99,'Unpaid','2025-03-07 03:29:55','2025-03-07 03:30:20'),(2,2,1,'iPhone 15 Pro',2,999.99,1999.98,'Unpaid','2025-03-06 21:36:05','2025-03-07 05:36:05');
/*!40000 ALTER TABLE `orders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `products`
--

DROP TABLE IF EXISTS `products`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `products` (
  `ProductId` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) DEFAULT NULL,
  `Description` text,
  `Price` decimal(10,2) NOT NULL,
  `StockQuantity` int NOT NULL DEFAULT '0',
  `CategoryId` int DEFAULT NULL,
  `CreatedAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `UpdatedAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `picture` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`ProductId`),
  KEY `CategoryId` (`CategoryId`),
  CONSTRAINT `products_ibfk_1` FOREIGN KEY (`CategoryId`) REFERENCES `categories` (`CategoryId`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `products`
--

LOCK TABLES `products` WRITE;
/*!40000 ALTER TABLE `products` DISABLE KEYS */;
INSERT INTO `products` VALUES (1,'iPhone 15 Pro','最新款苹果手机，搭载A17芯片，4800万像素主摄',999.99,46,2,'2025-03-06 07:42:56','2025-03-07 05:36:42',NULL),(2,'MacBook Air M2','搭载M2芯片的轻薄笔记本，续航持久',1299.99,30,7,'2025-03-06 07:42:56','2025-03-07 02:24:44',NULL),(3,'AirPods Pro','主动降噪无线耳机，空间音频',249.99,100,2,'2025-03-06 07:42:56','2025-03-07 02:24:44',NULL),(4,'男士休闲夹克','春秋季节百搭外套，防风保暖',199.99,80,8,'2025-03-06 07:42:56','2025-03-07 02:24:44',NULL),(5,'女士连衣裙','夏季碎花连衣裙，舒适透气',159.99,60,8,'2025-03-06 07:42:56','2025-03-07 02:24:44',NULL),(6,'运动休闲裤','弹力面料，适合运动休闲',89.99,120,8,'2025-03-06 07:42:56','2025-03-07 02:24:44',NULL),(7,'有机水果礼盒','精选当季水果，营养美味',99.99,40,6,'2025-03-06 07:42:56','2025-03-07 02:24:44',NULL),(8,'进口零食大礼包','多种进口零食组合',149.99,70,9,'2025-03-06 07:42:56','2025-03-07 02:24:44',NULL),(9,'坚果混合包','每日坚果，营养均衡',79.99,90,9,'2025-03-06 07:42:56','2025-03-07 02:24:44',NULL),(10,'智能台灯','可调节亮度色温，护眼设计',129.99,45,1,'2025-03-06 07:42:56','2025-03-07 02:24:44',NULL),(11,'简约沙发','北欧风格，舒适耐用',1999.99,15,10,'2025-03-06 07:42:56','2025-03-07 02:24:44',NULL),(12,'收纳置物架','多层设计，空间利用',89.99,60,10,'2025-03-06 07:42:56','2025-03-07 02:24:44',NULL),(13,'Python编程入门','零基础入门Python编程',79.99,40,11,'2025-03-06 07:42:56','2025-03-07 02:24:44',NULL),(14,'世界名著全集','精装版世界文学名著',299.99,25,11,'2025-03-06 07:42:56','2025-03-07 02:24:44',NULL),(15,'儿童绘本套装','趣味性强的儿童绘本',129.99,50,11,'2025-03-06 07:42:56','2025-03-07 02:24:44',NULL),(16,'瑜伽垫','环保材质，防滑耐用',49.99,100,3,'2025-03-06 07:42:56','2025-03-07 02:24:44',NULL),(17,'哑铃套装','可调节重量，家用健身',199.99,30,3,'2025-03-06 07:42:56','2025-03-07 02:24:44',NULL),(18,'跑步机','静音设计，多功能显示',2999.99,10,3,'2025-03-06 07:42:56','2025-03-07 02:24:44',NULL),(19,'护肤套装','补水保湿，改善肤质',299.99,40,3,'2025-03-06 07:42:56','2025-03-07 02:24:44',NULL),(20,'口红礼盒','多色号组合，持久显色',199.99,50,3,'2025-03-06 07:42:56','2025-03-07 02:24:44',NULL),(21,'香水套装','淡雅香型，持久留香',399.99,30,3,'2025-03-06 07:42:56','2025-03-07 02:24:44',NULL),(22,'智能手表','心率监测，运动追踪',399.99,35,2,'2025-03-06 07:42:56','2025-03-07 02:24:44',NULL),(23,'休闲运动鞋','轻便舒适，时尚百搭',159.99,75,8,'2025-03-06 07:42:56','2025-03-07 02:24:44',NULL),(24,'手机','更新后的商品描述',199.99,0,2,'2025-03-06 01:32:13','2025-03-07 02:24:44',''),(29,'手机','更新后的商品描述',199.99,0,2,'2025-03-06 17:16:02','2025-03-07 02:24:44',''),(30,'手机','更新后的商品描述',199.99,50,2,'2025-03-06 17:24:52','2025-03-07 02:24:44','');
/*!40000 ALTER TABLE `products` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tokens`
--

DROP TABLE IF EXISTS `tokens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tokens` (
  `TokenId` varchar(255) NOT NULL,
  `UserId` int NOT NULL,
  `LoginTime` timestamp NOT NULL,
  `ExpiryTime` timestamp NOT NULL,
  PRIMARY KEY (`TokenId`),
  KEY `UserId` (`UserId`),
  CONSTRAINT `tokens_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tokens`
--

LOCK TABLES `tokens` WRITE;
/*!40000 ALTER TABLE `tokens` DISABLE KEYS */;
INSERT INTO `tokens` VALUES ('35ad3e17a90748cc5e9b99dfc0f1abf1874cae30b6026fd519babbdd0ac30da6',1,'2025-03-06 19:05:44','2025-03-13 19:05:44'),('9452626b1f570812ab9ce44dada63561e596c690912facce122fcb7de9a84e03',2,'2025-03-06 19:06:34','2025-03-13 19:06:34');
/*!40000 ALTER TABLE `tokens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `UserId` int NOT NULL AUTO_INCREMENT,
  `Username` varchar(50) NOT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `PasswordHash` varchar(255) NOT NULL,
  `Role` enum('User','Admin') DEFAULT 'User',
  `PointsBalance` int DEFAULT '0',
  `CreatedAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `UpdatedAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `avatar` varchar(255) DEFAULT NULL COMMENT 'user avatar',
  PRIMARY KEY (`UserId`),
  UNIQUE KEY `Username` (`Username`),
  KEY `Email` (`Email`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'zhang','abc@cc.com','ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad','Admin',100000,'2025-03-04 07:30:41','2025-03-05 22:18:06','string'),(2,'wangwu','12345@abc.com','5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5','User',20,'2025-03-06 01:39:41','2025-03-06 18:58:24',''),(7,'Ryan','12345@abc.com','5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5','User',20,'2025-03-05 19:07:16','2025-03-06 19:05:07',''),(9,'Tony','12345@abc.com','5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5','User',20,'2025-03-06 06:59:04','2025-03-06 19:09:45',''),(10,'王五','123@abc.com','a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3','User',20,'2025-03-06 18:34:38','2025-03-06 18:34:38',''),(12,'张三','12345@abc.com','5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5','User',20,'2025-03-06 18:38:51','2025-03-06 18:38:51','');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-03-07 17:17:53
