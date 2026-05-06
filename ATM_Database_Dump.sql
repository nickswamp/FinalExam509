-- MySQL dump for ATM_System
-- ------------------------------------------------------
-- Host: localhost    Database: ATM_System

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;

-- Create Database if it doesn't exist
CREATE DATABASE IF NOT EXISTS `ATM_System`;
USE `ATM_System`;

-- Table structure for table `accounts`
DROP TABLE IF EXISTS `accounts`;
CREATE TABLE `accounts` (
  `AccountID` int NOT NULL AUTO_INCREMENT,
  `Login` varchar(50) NOT NULL,
  `PinCode` varchar(5) NOT NULL,
  `Role` enum('Customer','Administrator') NOT NULL,
  `HolderName` varchar(100) DEFAULT NULL,
  `Balance` decimal(15,2) DEFAULT '0.00',
  `Status` enum('Active','Disabled') DEFAULT 'Active',
  PRIMARY KEY (`AccountID`),
  UNIQUE KEY `Login` (`Login`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Initial data for testing
LOCK TABLES `accounts` WRITE;
INSERT INTO `accounts` VALUES
(1,'Admin01','55555','Administrator','System Admin',0.00,'Active'),
(2,'Adnan123','12345','Customer','Sample User',178145.00,'Active');
UNLOCK TABLES;
