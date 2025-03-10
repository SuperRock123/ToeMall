-- MySQL dump 10.13  Distrib 8.0.29, for Win64 (x86_64)
--
-- Host: localhost    Database: toemall
-- ------------------------------------------------------
-- Server version	8.0.29

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
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categories`
--

LOCK TABLES `categories` WRITE;
/*!40000 ALTER TABLE `categories` DISABLE KEYS */;
INSERT INTO `categories` VALUES (1,'电器','生活电器'),(2,'手机','手机'),(3,'日用','日用百货'),(4,'速食','速食'),(5,'生鲜','生鲜'),(6,'水果','水果'),(7,'电脑','电脑'),(8,'衣物','时尚穿搭'),(9,'零食','零食'),(10,'家具','家具'),(11,'学习','学习'),(12,'体育','体育运动用品');
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
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orders`
--

LOCK TABLES `orders` WRITE;
/*!40000 ALTER TABLE `orders` DISABLE KEYS */;
INSERT INTO `orders` VALUES (1,1,1,'iPhone 15 Pro',1,999.99,999.99,'Paid','2025-03-07 03:29:55','2025-03-09 05:56:29'),(2,2,1,'iPhone 15 Pro',2,999.99,1999.98,'Cancelled','2025-03-06 21:36:05','2025-03-09 05:56:36'),(13,17,49,'川崎羽毛球拍',2,219.99,439.98,'Cancelled','2025-03-09 22:08:38','2025-03-10 06:09:07'),(14,17,49,'川崎羽毛球拍',1,219.99,219.99,'Cancelled','2025-03-09 22:09:27','2025-03-10 06:13:06'),(15,17,45,'变频洗衣机',1,1599.00,1599.00,'Unpaid','2025-03-09 22:13:21','2025-03-10 06:13:21');
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
) ENGINE=InnoDB AUTO_INCREMENT=52 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `products`
--

LOCK TABLES `products` WRITE;
/*!40000 ALTER TABLE `products` DISABLE KEYS */;
INSERT INTO `products` VALUES (1,'iPhone 15 Pro','最新款苹果手机，搭载A17芯片，4800万像素主摄',999.99,46,2,'2025-03-06 07:42:56','2025-03-10 05:01:26','https://img13.360buyimg.com/n7/jfs/t1/262550/20/23097/43293/67b9ae6fF446ce40b/860efaa1179dbb72.png'),(2,'MacBook Air M2','搭载M2芯片的轻薄笔记本，续航持久',1299.99,783,7,'2025-03-06 07:42:56','2025-03-10 06:19:26','https://img12.360buyimg.com/n7/jfs/t1/250376/23/38559/87300/67c977c4F5bd7fe53/181299e1cb9766af.png.avif'),(3,'AirPods Pro','主动降噪无线耳机，空间音频',249.99,100,2,'2025-03-06 07:42:56','2025-03-10 05:03:45','https://img11.360buyimg.com/n7/jfs/t1/255802/31/15901/60447/67934e56F9124722e/81404a54534a74b0.jpg.avif'),(5,'女士连衣裙','夏季碎花连衣裙，舒适透气',159.99,60,8,'2025-03-06 07:42:56','2025-03-10 06:02:17','https://img13.360buyimg.com/n7/jfs/t1/181291/19/44288/63244/6641ece6Fc1f47023/5efb09a8be67a0dc.jpg.avif'),(6,'运动休闲裤','弹力面料，适合运动休闲',89.99,120,8,'2025-03-06 07:42:56','2025-03-10 06:02:17','https://img11.360buyimg.com/n7/jfs/t1/205394/37/44013/44592/6700b805F5bbc9328/11aa5fb081235769.jpg.avif'),(7,'有机水果礼盒','精选当季水果，营养美味',99.99,40,6,'2025-03-10 05:35:56','2025-03-10 05:35:56','https://img11.360buyimg.com/babel/s480x480_jfs/t1/108833/13/48920/131051/66e4f3deF124a8dce/8079bf5481cfcd0d.jpg.avif'),(8,'进口零食大礼包','多种进口零食组合',149.99,70,9,'2025-03-06 07:42:56','2025-03-10 06:04:11','https://img11.360buyimg.com/n7/jfs/t1/5541/29/23429/356203/66908599Fae97eeb7/9fba75d97571c2c6.jpg.avif'),(9,'坚果混合包','每日坚果，营养均衡',79.99,90,9,'2025-03-06 07:42:56','2025-03-10 05:45:45','https://img10.360buyimg.com/babel/s480x480_jfs/t1/219554/39/42455/81559/665e75c2F5b540b05/939d4d27ad8f39bb.jpg.avif'),(10,'智能台灯','可调节亮度色温，护眼设计',129.99,45,1,'2025-03-06 07:42:56','2025-03-10 05:10:33','https://img13.360buyimg.com/n7/jfs/t1/278710/19/198/104440/67ce3edaFc726b47c/e28c8d230c40fe27.png.avif'),(11,'空调被','北欧风格，舒适耐用',90.99,15,10,'2025-03-06 07:42:56','2025-03-10 05:51:30','https://img13.360buyimg.com/n1/s450x450_jfs/t1/270697/13/831/112005/67ce7b23F90565f11/bf7fd680c1d62dd4.jpg.avif'),(12,'富氢净饮机','碧云泉N9【国家补贴】富氢净饮机免安装净煮速热茶饮水一体机台...',12899.99,60,10,'2025-03-06 07:42:56','2025-03-10 05:51:30','https://img10.360buyimg.com/n1/s450x450_jfs/t1/253978/28/18937/89230/67a73968F841041a0/1a7f404b38350eb4.jpg.avif'),(13,'Python编程入门','零基础入门Python编程',79.99,40,11,'2025-03-06 07:42:56','2025-03-10 05:54:06','https://img14.360buyimg.com/n7/jfs/t1/187536/31/42622/92600/65091a77F49dc7229/497f6d631fc7a7c2.jpg.avif'),(14,'世界名著全集','精装版世界文学名著',299.99,25,11,'2025-03-06 07:42:56','2025-03-10 05:54:06','https://img12.360buyimg.com/n7/jfs/t1/155991/6/21574/119035/61760a9fEbe11a970/a55c4c38d77aca93.jpg.avif'),(15,'儿童绘本套装','趣味性强的儿童绘本',129.99,50,11,'2025-03-06 07:42:56','2025-03-10 05:54:06','https://img13.360buyimg.com/n7/jfs/t1/105429/24/49393/138931/66c543e4Fdce54fd2/78722a530fd1ea9a.jpg.avif'),(16,'瑜伽垫','环保材质，防滑耐用',49.99,100,3,'2025-03-06 07:42:56','2025-03-10 05:15:32','https://img11.360buyimg.com/n7/jfs/t1/256864/33/24416/63278/67bbc627Fca65c95b/3282887e0ddd6880.jpg.avif'),(17,'哑铃套装','可调节重量，家用健身',199.99,30,3,'2025-03-06 07:42:56','2025-03-10 05:15:32','https://img11.360buyimg.com/n7/jfs/t1/2537/38/25241/103203/670dd6b9F03292ae3/830392608642b32c.jpg.avif'),(18,'跑步机','静音设计，多功能显示',2999.99,10,3,'2025-03-06 07:42:56','2025-03-10 05:15:32','https://img11.360buyimg.com/n7/jfs/t1/264425/9/29453/149965/67ca9e7cFd77301b8/eb416c26d357eff1.jpg.avif'),(19,'护肤套装','补水保湿，改善肤质',299.99,40,3,'2025-03-06 07:42:56','2025-03-10 05:15:32','https://img11.360buyimg.com/n7/jfs/t1/263408/19/30831/70123/67ccf0e3Fc6ff853c/ef7a6ac7147c2672.jpg.avif'),(20,'口红礼盒','多色号组合，持久显色',199.99,50,3,'2025-03-06 07:42:56','2025-03-10 05:15:32','https://img11.360buyimg.com/n7/jfs/t1/173721/39/47697/97235/6734d015F96dddc87/ac171e77deed66d3.jpg.avif'),(21,'香水套装','淡雅香型，持久留香',399.99,30,3,'2025-03-06 07:42:56','2025-03-10 05:15:32','https://img11.360buyimg.com/n7/jfs/t1/260404/11/25146/52927/67bfd212Fa1566138/05dc209ebbd1ae2f.jpg.avif'),(22,'智能手表','心率监测，运动追踪',399.99,35,2,'2025-03-06 07:42:56','2025-03-10 05:03:45','https://img14.360buyimg.com/n7/jfs/t1/260564/36/30200/42874/67cc2bbdF7477ead0/2c18272022af83e1.png.avif'),(23,'休闲运动鞋','轻便舒适，时尚百搭',159.99,75,8,'2025-03-06 07:42:56','2025-03-10 06:02:17','https://img14.360buyimg.com/n7/jfs/t1/260703/16/26125/131816/67c2cd77Fa23a94b5/c38c0d6c70ec5928.jpg.avif'),(36,'苹果 Watch Series 10','智能手表',388.88,200,2,'2025-03-10 05:05:32','2025-03-10 05:05:32','https://img14.360buyimg.com/n7/jfs/t1/260564/36/30200/42874/67cc2bbdF7477ead0/2c18272022af83e1.png.avif'),(37,'海尔KFR-35GW','【小红花套系】 劲爽1.5匹一级能效',888.99,20,1,'2025-03-10 05:10:06','2025-03-10 05:10:06','https://img14.360buyimg.com/n7/jfs/t1/264452/18/29895/110326/67cbd205F992b235b/1e954b974c21b76c.jpg.avif'),(38,'京东京造JS-383','京东京造汽车应急启动电源充气泵一体机搭电宝电瓶充电器户外电源...',229.99,23,1,'2025-03-10 05:10:06','2025-03-10 05:11:35','https://img12.360buyimg.com/n7/jfs/t1/254602/40/29526/115914/67c8fb4bF0ed815b4/9f4dd24d8786b5cb.jpg.avif'),(39,'饭小馋小馄饨',' 饭小馋混合口味小馄饨抄手 6桶362g 鸡汤虾米红油整箱馄饨...',20.23,300,4,'2025-03-10 05:30:00','2025-03-10 05:30:00','https://img11.360buyimg.com/n7/jfs/t1/240021/1/6379/160896/661cd8dcFae64edcb/8b2f54a5632be618.jpg.avif'),(40,'手工老面小笼包','皇家小虎嵊州风味手工老面小笼包1000g约40个儿童早餐半成...',19.98,209,4,'2025-03-10 05:30:00','2025-03-10 06:03:27','https://img11.360buyimg.com/n7/jfs/t1/256294/36/12464/186262/678487caF3d2706a9/bf944652233053a5.jpg.avif'),(41,'厄瓜多尔白虾','鲜京采 厄瓜多尔白虾 净重3斤/盒 特大号20-30规格 ...',39.88,4000,5,'2025-03-10 05:32:27','2025-03-10 05:32:27','https://img11.360buyimg.com/n7/jfs/t1/262779/20/6748/152056/6775e67dFe6af2c55/14d367d3f34d1296.jpg.avif'),(42,'巴西进口原切胸部牛肋肉','鲜京采 巴西进口原切胸部牛肋肉 2斤 牛肋条 炖煮烧烤 京东...',88.88,680,5,'2025-03-10 05:32:27','2025-03-10 05:32:27','https://img14.360buyimg.com/n7/jfs/t1/258746/3/4025/100179/676e41e1Fb2a96ab2/611ccf47c191122b.jpg.avif'),(43,'广西武鸣沃柑','京鲜生 广西武鸣沃柑 净重5斤装 单果110g+ 生鲜水果 ...',28.99,890,6,'2025-03-10 05:35:56','2025-03-10 05:35:56','https://img13.360buyimg.com/babel/s480x480_jfs/t20251202/246953/39/26431/78366/674d2ce3F6192d37c/d07ed508595838e4.jpg.avif'),(44,'蛋白面包棒','良品铺子乳清蛋白面包棒500g学生营养早餐整箱吐司夹心零食牛...',34.99,600,9,'2025-03-10 05:47:00','2025-03-10 05:47:00','https://img10.360buyimg.com/babel/s480x480_jfs/t20270711/237195/34/19836/57612/668f3fffF23c65dc1/bd0fc5e324ef6b29.jpg.avif'),(45,'变频洗衣机','小天鹅（LittleSwan）直驱变频洗衣机全自动波轮大容量...',1599.00,399,10,'2025-03-10 05:49:23','2025-03-10 06:13:21','https://img11.360buyimg.com/n1/s450x450_jfs/t1/8859/22/26964/162000/66ebf3e4F6a93634a/6700071498070b3d.jpg.avif'),(46,'胜利NS3000','塑料羽毛球耐打稳定6只装尼龙羽毛球',39.98,309,12,'2025-03-10 05:56:08','2025-03-10 05:56:08','https://img12.360buyimg.com/n1/jfs/t1/68809/32/21285/56775/62d7827eE77e18a76/2b03272436cd28de.jpg.avif'),(47,'YY耐打AS9','YONEX 尤尼克斯羽毛球',45.54,402,12,'2025-03-10 05:57:28','2025-03-10 05:57:28','https://img13.360buyimg.com/n1/jfs/t1/229656/14/4536/95541/6562ce3aF831e859e/f0103ee387f489ae.jpg.avif'),(48,'均衡之刃','的幸（DRACAENA）黄厂均衡之刃全碳素纤维材质羽毛球进攻耐打初学者新手耐用单拍 皓月白 成品拍',99.00,892,12,'2025-03-10 05:59:14','2025-03-10 05:59:14','https://img12.360buyimg.com/n1/jfs/t1/252619/22/25427/150046/67c3ce1aFe80b6400/516514977a2bb130.jpg.avif'),(49,'川崎羽毛球拍','女武神-紫(紫色线)6u进攻拍 22-23磅',219.99,321,12,'2025-03-10 06:00:26','2025-03-10 06:13:06','https://img12.360buyimg.com/n1/jfs/t1/238101/10/22203/85329/66b39d6bF400f5339/094f613dd9337751.jpg.avif'),(50,'华为MateBook','华为MateBook D 14 SE 2023笔记本电脑 国家补贴20% 12代酷睿/14英寸护眼屏i5 16G 512G 皓月银',3299.00,489,7,'2025-03-10 06:07:32','2025-03-10 06:07:32','https://img12.360buyimg.com/n1/s450x450_jfs/t1/267696/32/19782/76569/67aeef39F7da14ad6/2ba915a571167f0f.jpg.avif'),(51,'联想拯救者Y9000P','可选RTX4060满血独显电竞游戏笔记本电脑 大学生商务设计本 i7-13700H 标压蓝光护眼屏',6399.20,897,7,'2025-03-10 06:07:32','2025-03-10 06:07:32','https://img11.360buyimg.com/n1/jfs/t1/235389/30/36371/585535/67c7f029F91906271/49780473c1501f75.png.avif');
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
INSERT INTO `tokens` VALUES ('29dbedac6d139c8eafdecc4e7d7d2eba9e05395237d61b9b8ca78edfab8caf84',1,'2025-03-09 17:52:41','2025-03-16 17:52:41'),('aeeb0af07ae0ea1000273c21e864180eb5bbb63d3325f7763b515b81859a055e',2,'2025-03-09 00:54:18','2025-03-16 00:54:18'),('f1de5acffa67fa094e8c479e4e73cb02c51e8a2efb6946547fb9d9976231bc42',17,'2025-03-09 20:54:39','2025-03-16 20:54:39');
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
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'zhang','abc@cc.com','ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad','Admin',100000,'2025-03-04 07:30:41','2025-03-09 00:53:05','https://cdn.pixabay.com/photo/2025/03/01/17/39/tree-branch-9440514_1280.jpg'),(2,'wangwu','12345@abc.com','5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5','User',22,'2025-03-06 01:39:41','2025-03-10 01:39:26',''),(7,'Ryan','12345@abc.com','5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5','User',20,'2025-03-05 19:07:16','2025-03-06 19:05:07',''),(9,'Tony','12345@abc.com','5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5','User',20,'2025-03-06 06:59:04','2025-03-06 19:09:45',''),(10,'王五','123@abc.com','a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3','User',20,'2025-03-06 18:34:38','2025-03-06 18:34:38',''),(12,'张三','12345@abc.com','5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5','User',20,'2025-03-06 18:38:51','2025-03-10 07:12:13','https://cdn.pixabay.com/photo/2023/04/10/12/43/bird-7913707_1280.jpg'),(17,'user','abcd.comk','04f8996da763b7a969b1028ee3007569eaf3a635486ddab211d512c85b9df8fb','User',20,'2025-03-09 20:53:49','2025-03-10 06:13:06','https://cdn.pixabay.com/photo/2023/07/05/11/14/alpaca-8108043_1280.png');
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

-- Dump completed on 2025-03-10 18:50:45
