CREATE DATABASE  IF NOT EXISTS `edubot` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `edubot`;
-- MySQL dump 10.13  Distrib 8.0.33, for Win64 (x86_64)
--
-- Host: localhost    Database: edubot
-- ------------------------------------------------------
-- Server version	8.2.0

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `courses`
--

DROP TABLE IF EXISTS `courses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `courses` (
  `CourseName` varchar(45) NOT NULL,
  PRIMARY KEY (`CourseName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `courses`
--

LOCK TABLES `courses` WRITE;
/*!40000 ALTER TABLE `courses` DISABLE KEYS */;
INSERT INTO `courses` VALUES ('liquidator'),('simulator'),('transformer');
/*!40000 ALTER TABLE `courses` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `groups`
--

DROP TABLE IF EXISTS `groups`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `groups` (
  `GroupNumber` varchar(45) NOT NULL,
  `Password` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`GroupNumber`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `groups`
--

LOCK TABLES `groups` WRITE;
/*!40000 ALTER TABLE `groups` DISABLE KEYS */;
INSERT INTO `groups` VALUES ('1111111-11111','ioylim');
/*!40000 ALTER TABLE `groups` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `groupscourses`
--

DROP TABLE IF EXISTS `groupscourses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `groupscourses` (
  `GroupNumber` varchar(45) DEFAULT NULL,
  `CourseName` varchar(45) DEFAULT NULL,
  KEY `GroupNumber_idx` (`GroupNumber`),
  KEY `CourseName_idx` (`CourseName`),
  CONSTRAINT `CourseName` FOREIGN KEY (`CourseName`) REFERENCES `courses` (`CourseName`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `GroupNumber` FOREIGN KEY (`GroupNumber`) REFERENCES `groups` (`GroupNumber`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `groupscourses`
--

LOCK TABLES `groupscourses` WRITE;
/*!40000 ALTER TABLE `groupscourses` DISABLE KEYS */;
INSERT INTO `groupscourses` VALUES ('1111111-11111','simulator'),('1111111-11111','transformer'),('1111111-11111','liquidator');
/*!40000 ALTER TABLE `groupscourses` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `statsliquidatoranswers`
--

DROP TABLE IF EXISTS `statsliquidatoranswers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `statsliquidatoranswers` (
  `UserID` int NOT NULL,
  `P1M1A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P2M1A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P3M1A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P4M1A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P5M1A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P6M1A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P7M1A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P8M1A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P10M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P11M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P12M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P13M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P14M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P15M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P16M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P17M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P18M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P19M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P20M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P21M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P23M3A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P24M3A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P25M3A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P26M3A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P27M3A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P1M1A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P2M1A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P3M1A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P4M1A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P5M1A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P6M1A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P7M1A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P8M1A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P10M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P11M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P12M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P13M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P14M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P15M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P16M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P17M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P18M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P19M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P20M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P21M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P23M3A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P24M3A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P25M3A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P26M3A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P27M3A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  PRIMARY KEY (`UserID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `statsliquidatoranswers`
--

LOCK TABLES `statsliquidatoranswers` WRITE;
/*!40000 ALTER TABLE `statsliquidatoranswers` DISABLE KEYS */;
INSERT INTO `statsliquidatoranswers` VALUES (438910696,'5;6;7','3','3','2;3;4','2;3;5;6','1;2;4;5','0','2;3','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','4;5;7','4','4','1;4;5','0;1;3;6','3;4;5;7','0','2;3','4','2;4;6','4','0','1;3;5','1;2;4','2;3;4','1','5','3','4','3','1','5','0','4','1'),(750988303,'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'),(870167626,'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'),(1034515395,'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'),(1096972354,'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'),(1729203905,'6','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0');
/*!40000 ALTER TABLE `statsliquidatoranswers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `statsliquidatorbase`
--

DROP TABLE IF EXISTS `statsliquidatorbase`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `statsliquidatorbase` (
  `UserID` int NOT NULL,
  `StartCourseTime` datetime DEFAULT NULL,
  `EndCourseTime` datetime DEFAULT NULL,
  `AttemptsUsed` int NOT NULL DEFAULT '0',
  `RateAttempt1` double NOT NULL DEFAULT '0',
  `RateAttempt2` double NOT NULL DEFAULT '0',
  PRIMARY KEY (`UserID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `statsliquidatorbase`
--

LOCK TABLES `statsliquidatorbase` WRITE;
/*!40000 ALTER TABLE `statsliquidatorbase` DISABLE KEYS */;
INSERT INTO `statsliquidatorbase` VALUES (438910696,'2024-02-22 11:13:20','2024-02-22 11:16:45',2,14,35),(750988303,NULL,NULL,0,0,0),(870167626,NULL,NULL,0,0,0),(1034515395,NULL,NULL,0,0,0),(1096972354,NULL,NULL,0,0,0),(1729203905,'2024-02-22 10:51:03',NULL,0,0,0);
/*!40000 ALTER TABLE `statsliquidatorbase` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `statsliquidatorrate`
--

DROP TABLE IF EXISTS `statsliquidatorrate`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `statsliquidatorrate` (
  `UserID` int NOT NULL,
  `P1M1A1` double NOT NULL DEFAULT '0',
  `P2M1A1` double NOT NULL DEFAULT '0',
  `P3M1A1` double NOT NULL DEFAULT '0',
  `P4M1A1` double NOT NULL DEFAULT '0',
  `P5M1A1` double NOT NULL DEFAULT '0',
  `P6M1A1` double NOT NULL DEFAULT '0',
  `P7M1A1` double NOT NULL DEFAULT '0',
  `P8M1A1` double NOT NULL DEFAULT '0',
  `P10M2A1` double NOT NULL DEFAULT '0',
  `P11M2A1` double NOT NULL DEFAULT '0',
  `P12M2A1` double NOT NULL DEFAULT '0',
  `P13M2A1` double NOT NULL DEFAULT '0',
  `P14M2A1` double NOT NULL DEFAULT '0',
  `P15M2A1` double NOT NULL DEFAULT '0',
  `P16M2A1` double NOT NULL DEFAULT '0',
  `P17M2A1` double NOT NULL DEFAULT '0',
  `P18M2A1` double NOT NULL DEFAULT '0',
  `P19M2A1` double NOT NULL DEFAULT '0',
  `P20M2A1` double NOT NULL DEFAULT '0',
  `P21M2A1` double NOT NULL DEFAULT '0',
  `P23M3A1` double NOT NULL DEFAULT '0',
  `P24M3A1` double NOT NULL DEFAULT '0',
  `P25M3A1` double NOT NULL DEFAULT '0',
  `P26M3A1` double NOT NULL DEFAULT '0',
  `P27M3A1` double NOT NULL DEFAULT '0',
  `P1M1A2` double NOT NULL DEFAULT '0',
  `P2M1A2` double NOT NULL DEFAULT '0',
  `P3M1A2` double NOT NULL DEFAULT '0',
  `P4M1A2` double NOT NULL DEFAULT '0',
  `P5M1A2` double NOT NULL DEFAULT '0',
  `P6M1A2` double NOT NULL DEFAULT '0',
  `P7M1A2` double NOT NULL DEFAULT '0',
  `P8M1A2` double NOT NULL DEFAULT '0',
  `P10M2A2` double NOT NULL DEFAULT '0',
  `P11M2A2` double NOT NULL DEFAULT '0',
  `P12M2A2` double NOT NULL DEFAULT '0',
  `P13M2A2` double NOT NULL DEFAULT '0',
  `P14M2A2` double NOT NULL DEFAULT '0',
  `P15M2A2` double NOT NULL DEFAULT '0',
  `P16M2A2` double NOT NULL DEFAULT '0',
  `P17M2A2` double NOT NULL DEFAULT '0',
  `P18M2A2` double NOT NULL DEFAULT '0',
  `P19M2A2` double NOT NULL DEFAULT '0',
  `P20M2A2` double NOT NULL DEFAULT '0',
  `P21M2A2` double NOT NULL DEFAULT '0',
  `P23M3A2` double NOT NULL DEFAULT '0',
  `P24M3A2` double NOT NULL DEFAULT '0',
  `P25M3A2` double NOT NULL DEFAULT '0',
  `P26M3A2` double NOT NULL DEFAULT '0',
  `P27M3A2` double NOT NULL DEFAULT '0',
  PRIMARY KEY (`UserID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `statsliquidatorrate`
--

LOCK TABLES `statsliquidatorrate` WRITE;
/*!40000 ALTER TABLE `statsliquidatorrate` DISABLE KEYS */;
INSERT INTO `statsliquidatorrate` VALUES (438910696,-1,2,3,3,-1,5,4,-1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0.5,3,1.5,-1,3,5.5,4,-1,2,4,-0.5,0,3,4,4,2,0,1.5,0.5,-1,0,2,0,-1,-1),(750988303,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(870167626,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(1034515395,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(1096972354,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(1729203905,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0);
/*!40000 ALTER TABLE `statsliquidatorrate` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `statsliquidatorstate`
--

DROP TABLE IF EXISTS `statsliquidatorstate`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `statsliquidatorstate` (
  `UserID` int NOT NULL,
  `Point` int NOT NULL DEFAULT '0',
  `ExtraAttempt` tinyint(1) DEFAULT NULL,
  `Attempts` int DEFAULT NULL,
  `Rate` double NOT NULL DEFAULT '0',
  `StartTime` datetime DEFAULT NULL,
  PRIMARY KEY (`UserID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `statsliquidatorstate`
--

LOCK TABLES `statsliquidatorstate` WRITE;
/*!40000 ALTER TABLE `statsliquidatorstate` DISABLE KEYS */;
INSERT INTO `statsliquidatorstate` VALUES (438910696,-1,0,0,35,'2024-02-22 11:16:44'),(750988303,0,1,2,0,NULL),(870167626,0,1,2,0,NULL),(1034515395,0,1,2,0,NULL),(1096972354,0,1,2,0,NULL),(1729203905,2,1,2,0,'2024-02-22 10:51:18');
/*!40000 ALTER TABLE `statsliquidatorstate` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `statsliquidatortime`
--

DROP TABLE IF EXISTS `statsliquidatortime`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `statsliquidatortime` (
  `UserID` int NOT NULL,
  `P1M1A1` double NOT NULL DEFAULT '0',
  `P2M1A1` double NOT NULL DEFAULT '0',
  `P3M1A1` double NOT NULL DEFAULT '0',
  `P4M1A1` double NOT NULL DEFAULT '0',
  `P5M1A1` double NOT NULL DEFAULT '0',
  `P6M1A1` double NOT NULL DEFAULT '0',
  `P7M1A1` double NOT NULL DEFAULT '0',
  `P8M1A1` double NOT NULL DEFAULT '0',
  `P10M2A1` double NOT NULL DEFAULT '0',
  `P11M2A1` double NOT NULL DEFAULT '0',
  `P12M2A1` double NOT NULL DEFAULT '0',
  `P13M2A1` double NOT NULL DEFAULT '0',
  `P14M2A1` double NOT NULL DEFAULT '0',
  `P15M2A1` double NOT NULL DEFAULT '0',
  `P16M2A1` double NOT NULL DEFAULT '0',
  `P17M2A1` double NOT NULL DEFAULT '0',
  `P18M2A1` double NOT NULL DEFAULT '0',
  `P19M2A1` double NOT NULL DEFAULT '0',
  `P20M2A1` double NOT NULL DEFAULT '0',
  `P21M2A1` double NOT NULL DEFAULT '0',
  `P23M3A1` double NOT NULL DEFAULT '0',
  `P24M3A1` double NOT NULL DEFAULT '0',
  `P25M3A1` double NOT NULL DEFAULT '0',
  `P26M3A1` double NOT NULL DEFAULT '0',
  `P27M3A1` double NOT NULL DEFAULT '0',
  `P1M1A2` double NOT NULL DEFAULT '0',
  `P2M1A2` double NOT NULL DEFAULT '0',
  `P3M1A2` double NOT NULL DEFAULT '0',
  `P4M1A2` double NOT NULL DEFAULT '0',
  `P5M1A2` double NOT NULL DEFAULT '0',
  `P6M1A2` double NOT NULL DEFAULT '0',
  `P7M1A2` double NOT NULL DEFAULT '0',
  `P8M1A2` double NOT NULL DEFAULT '0',
  `P10M2A2` double NOT NULL DEFAULT '0',
  `P11M2A2` double NOT NULL DEFAULT '0',
  `P12M2A2` double NOT NULL DEFAULT '0',
  `P13M2A2` double NOT NULL DEFAULT '0',
  `P14M2A2` double NOT NULL DEFAULT '0',
  `P15M2A2` double NOT NULL DEFAULT '0',
  `P16M2A2` double NOT NULL DEFAULT '0',
  `P17M2A2` double NOT NULL DEFAULT '0',
  `P18M2A2` double NOT NULL DEFAULT '0',
  `P19M2A2` double NOT NULL DEFAULT '0',
  `P20M2A2` double NOT NULL DEFAULT '0',
  `P21M2A2` double NOT NULL DEFAULT '0',
  `P23M3A2` double NOT NULL DEFAULT '0',
  `P24M3A2` double NOT NULL DEFAULT '0',
  `P25M3A2` double NOT NULL DEFAULT '0',
  `P26M3A2` double NOT NULL DEFAULT '0',
  `P27M3A2` double NOT NULL DEFAULT '0',
  PRIMARY KEY (`UserID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `statsliquidatortime`
--

LOCK TABLES `statsliquidatortime` WRITE;
/*!40000 ALTER TABLE `statsliquidatortime` DISABLE KEYS */;
INSERT INTO `statsliquidatortime` VALUES (438910696,0.118867135,0.041603553333333335,0.03441092166666666,0.05188395333333334,0.09130026,0.054207091666666665,0.19278699,0.08607950166666667,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0.04813562,0.028819921666666668,0.02672648,0.048525575,0.058641245,0.052020453333333334,0.16916982,0.03553943,0.025772658333333334,0.049457763333333335,0.034967165,0.02557056166666667,0.041988976666666664,0.042614195,0.03992149666666667,0.11080843166666667,0.037381011666666665,0.01445079,0.030674146666666666,0.026809096666666667,0.03812362166666667,0.016237795,0,0.030893675,0.025999316666666668),(750988303,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(870167626,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(1034515395,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(1096972354,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(1729203905,0.050253115,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0);
/*!40000 ALTER TABLE `statsliquidatortime` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `statssimulatoranswers`
--

DROP TABLE IF EXISTS `statssimulatoranswers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `statssimulatoranswers` (
  `UserID` int NOT NULL,
  `P1M1A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P2M1A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P3M1A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P4M1A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P5M1A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P6M1A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P7M1A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P8M1A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P10M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P11M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P12M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P13M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P14M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P15M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P16M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P17M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P18M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P19M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P20M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P21M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P23M3A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P24M3A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P25M3A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P26M3A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P27M3A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P1M1A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P2M1A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P3M1A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P4M1A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P5M1A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P6M1A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P7M1A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P8M1A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P10M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P11M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P12M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P13M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P14M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P15M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P16M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P17M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P18M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P19M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P20M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P21M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P23M3A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P24M3A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P25M3A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P26M3A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P27M3A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  PRIMARY KEY (`UserID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `statssimulatoranswers`
--

LOCK TABLES `statssimulatoranswers` WRITE;
/*!40000 ALTER TABLE `statssimulatoranswers` DISABLE KEYS */;
INSERT INTO `statssimulatoranswers` VALUES (438910696,'2;4;5;6','4','2','2;4;5','8','4;6;7','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'),(750988303,'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'),(870167626,'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'),(1034515395,'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'),(1096972354,'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'),(1729203905,'6','4','5','6','8','7','0','4','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','8','4','5','6','8','7','0','4','0','6','4','1','5','4','4','3','5','3','4','3','1','5','0','4','1');
/*!40000 ALTER TABLE `statssimulatoranswers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `statssimulatorbase`
--

DROP TABLE IF EXISTS `statssimulatorbase`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `statssimulatorbase` (
  `UserID` int NOT NULL,
  `StartCourseTime` datetime DEFAULT NULL,
  `EndCourseTime` datetime DEFAULT NULL,
  `AttemptsUsed` int NOT NULL DEFAULT '0',
  `RateAttempt1` double NOT NULL DEFAULT '0',
  `RateAttempt2` double NOT NULL DEFAULT '0',
  PRIMARY KEY (`UserID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `statssimulatorbase`
--

LOCK TABLES `statssimulatorbase` WRITE;
/*!40000 ALTER TABLE `statssimulatorbase` DISABLE KEYS */;
INSERT INTO `statssimulatorbase` VALUES (438910696,'2024-02-22 11:16:52',NULL,0,18.5,0),(750988303,NULL,NULL,0,0,0),(870167626,NULL,NULL,0,0,0),(1034515395,NULL,NULL,0,0,0),(1096972354,NULL,NULL,0,0,0),(1729203905,'2024-02-01 20:45:15','2024-02-01 20:55:39',2,7,-2);
/*!40000 ALTER TABLE `statssimulatorbase` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `statssimulatorrate`
--

DROP TABLE IF EXISTS `statssimulatorrate`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `statssimulatorrate` (
  `UserID` int NOT NULL,
  `P1M1A1` double NOT NULL DEFAULT '0',
  `P2M1A1` double NOT NULL DEFAULT '0',
  `P3M1A1` double NOT NULL DEFAULT '0',
  `P4M1A1` double NOT NULL DEFAULT '0',
  `P5M1A1` double NOT NULL DEFAULT '0',
  `P6M1A1` double NOT NULL DEFAULT '0',
  `P7M1A1` double NOT NULL DEFAULT '0',
  `P8M1A1` double NOT NULL DEFAULT '0',
  `P10M2A1` double NOT NULL DEFAULT '0',
  `P11M2A1` double NOT NULL DEFAULT '0',
  `P12M2A1` double NOT NULL DEFAULT '0',
  `P13M2A1` double NOT NULL DEFAULT '0',
  `P14M2A1` double NOT NULL DEFAULT '0',
  `P15M2A1` double NOT NULL DEFAULT '0',
  `P16M2A1` double NOT NULL DEFAULT '0',
  `P17M2A1` double NOT NULL DEFAULT '0',
  `P18M2A1` double NOT NULL DEFAULT '0',
  `P19M2A1` double NOT NULL DEFAULT '0',
  `P20M2A1` double NOT NULL DEFAULT '0',
  `P21M2A1` double NOT NULL DEFAULT '0',
  `P23M3A1` double NOT NULL DEFAULT '0',
  `P24M3A1` double NOT NULL DEFAULT '0',
  `P25M3A1` double NOT NULL DEFAULT '0',
  `P26M3A1` double NOT NULL DEFAULT '0',
  `P27M3A1` double NOT NULL DEFAULT '0',
  `P1M1A2` double NOT NULL DEFAULT '0',
  `P2M1A2` double NOT NULL DEFAULT '0',
  `P3M1A2` double NOT NULL DEFAULT '0',
  `P4M1A2` double NOT NULL DEFAULT '0',
  `P5M1A2` double NOT NULL DEFAULT '0',
  `P6M1A2` double NOT NULL DEFAULT '0',
  `P7M1A2` double NOT NULL DEFAULT '0',
  `P8M1A2` double NOT NULL DEFAULT '0',
  `P10M2A2` double NOT NULL DEFAULT '0',
  `P11M2A2` double NOT NULL DEFAULT '0',
  `P12M2A2` double NOT NULL DEFAULT '0',
  `P13M2A2` double NOT NULL DEFAULT '0',
  `P14M2A2` double NOT NULL DEFAULT '0',
  `P15M2A2` double NOT NULL DEFAULT '0',
  `P16M2A2` double NOT NULL DEFAULT '0',
  `P17M2A2` double NOT NULL DEFAULT '0',
  `P18M2A2` double NOT NULL DEFAULT '0',
  `P19M2A2` double NOT NULL DEFAULT '0',
  `P20M2A2` double NOT NULL DEFAULT '0',
  `P21M2A2` double NOT NULL DEFAULT '0',
  `P23M3A2` double NOT NULL DEFAULT '0',
  `P24M3A2` double NOT NULL DEFAULT '0',
  `P25M3A2` double NOT NULL DEFAULT '0',
  `P26M3A2` double NOT NULL DEFAULT '0',
  `P27M3A2` double NOT NULL DEFAULT '0',
  PRIMARY KEY (`UserID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `statssimulatorrate`
--

LOCK TABLES `statssimulatorrate` WRITE;
/*!40000 ALTER TABLE `statssimulatorrate` DISABLE KEYS */;
INSERT INTO `statssimulatorrate` VALUES (438910696,1.5,3,3,1,2,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(750988303,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(870167626,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(1034515395,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(1096972354,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(1729203905,0,3,0,3,2,1.5,4,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,0,3,2,1.5,4,3,2,0,-0.5,0,0.5,0.5,1,0.5,0,1.5,0.5,-1,0,2,0,-1,-1);
/*!40000 ALTER TABLE `statssimulatorrate` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `statssimulatorstate`
--

DROP TABLE IF EXISTS `statssimulatorstate`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `statssimulatorstate` (
  `UserID` int NOT NULL,
  `Point` int NOT NULL DEFAULT '0',
  `ExtraAttempt` tinyint(1) DEFAULT NULL,
  `Attempts` int DEFAULT NULL,
  `Rate` double NOT NULL DEFAULT '0',
  `StartTime` datetime DEFAULT NULL,
  PRIMARY KEY (`UserID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `statssimulatorstate`
--

LOCK TABLES `statssimulatorstate` WRITE;
/*!40000 ALTER TABLE `statssimulatorstate` DISABLE KEYS */;
INSERT INTO `statssimulatorstate` VALUES (438910696,8,1,2,18.5,'2024-02-22 11:24:32'),(750988303,0,1,2,0,NULL),(870167626,0,1,2,0,NULL),(1034515395,0,1,2,0,NULL),(1096972354,0,1,2,0,NULL),(1729203905,-1,0,0,-1,'2024-02-01 20:55:38');
/*!40000 ALTER TABLE `statssimulatorstate` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `statssimulatortime`
--

DROP TABLE IF EXISTS `statssimulatortime`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `statssimulatortime` (
  `UserID` int NOT NULL,
  `P1M1A1` double NOT NULL DEFAULT '0',
  `P2M1A1` double NOT NULL DEFAULT '0',
  `P3M1A1` double NOT NULL DEFAULT '0',
  `P4M1A1` double NOT NULL DEFAULT '0',
  `P5M1A1` double NOT NULL DEFAULT '0',
  `P6M1A1` double NOT NULL DEFAULT '0',
  `P7M1A1` double NOT NULL DEFAULT '0',
  `P8M1A1` double NOT NULL DEFAULT '0',
  `P10M2A1` double NOT NULL DEFAULT '0',
  `P11M2A1` double NOT NULL DEFAULT '0',
  `P12M2A1` double NOT NULL DEFAULT '0',
  `P13M2A1` double NOT NULL DEFAULT '0',
  `P14M2A1` double NOT NULL DEFAULT '0',
  `P15M2A1` double NOT NULL DEFAULT '0',
  `P16M2A1` double NOT NULL DEFAULT '0',
  `P17M2A1` double NOT NULL DEFAULT '0',
  `P18M2A1` double NOT NULL DEFAULT '0',
  `P19M2A1` double NOT NULL DEFAULT '0',
  `P20M2A1` double NOT NULL DEFAULT '0',
  `P21M2A1` double NOT NULL DEFAULT '0',
  `P23M3A1` double NOT NULL DEFAULT '0',
  `P24M3A1` double NOT NULL DEFAULT '0',
  `P25M3A1` double NOT NULL DEFAULT '0',
  `P26M3A1` double NOT NULL DEFAULT '0',
  `P27M3A1` double NOT NULL DEFAULT '0',
  `P1M1A2` double NOT NULL DEFAULT '0',
  `P2M1A2` double NOT NULL DEFAULT '0',
  `P3M1A2` double NOT NULL DEFAULT '0',
  `P4M1A2` double NOT NULL DEFAULT '0',
  `P5M1A2` double NOT NULL DEFAULT '0',
  `P6M1A2` double NOT NULL DEFAULT '0',
  `P7M1A2` double NOT NULL DEFAULT '0',
  `P8M1A2` double NOT NULL DEFAULT '0',
  `P10M2A2` double NOT NULL DEFAULT '0',
  `P11M2A2` double NOT NULL DEFAULT '0',
  `P12M2A2` double NOT NULL DEFAULT '0',
  `P13M2A2` double NOT NULL DEFAULT '0',
  `P14M2A2` double NOT NULL DEFAULT '0',
  `P15M2A2` double NOT NULL DEFAULT '0',
  `P16M2A2` double NOT NULL DEFAULT '0',
  `P17M2A2` double NOT NULL DEFAULT '0',
  `P18M2A2` double NOT NULL DEFAULT '0',
  `P19M2A2` double NOT NULL DEFAULT '0',
  `P20M2A2` double NOT NULL DEFAULT '0',
  `P21M2A2` double NOT NULL DEFAULT '0',
  `P23M3A2` double NOT NULL DEFAULT '0',
  `P24M3A2` double NOT NULL DEFAULT '0',
  `P25M3A2` double NOT NULL DEFAULT '0',
  `P26M3A2` double NOT NULL DEFAULT '0',
  `P27M3A2` double NOT NULL DEFAULT '0',
  PRIMARY KEY (`UserID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `statssimulatortime`
--

LOCK TABLES `statssimulatortime` WRITE;
/*!40000 ALTER TABLE `statssimulatortime` DISABLE KEYS */;
INSERT INTO `statssimulatortime` VALUES (438910696,1.0100577616666666,0.10606050666666667,0.058630126666666664,0.051325383333333335,0.023144193333333334,0.448849885,0.79203713,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(750988303,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(870167626,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(1034515395,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(1096972354,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(1729203905,0.04490238333333333,0.034289725,0.016602836666666666,0.02759191,0.037113136666666664,0.030286106666666666,0.353927775,0.04065535333333333,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0.047269905,0.029650198333333332,0.029593733333333334,0.04141244166666667,0.034490135,0.03282385,0.156330615,0.03902311,0.07504934333333334,0.038906025,0.022505336666666667,0.038485685,0.03005869166666667,0.036928688333333334,0.033781226666666664,0.06283024333333333,0.036506091666666664,0.018377478333333332,0.019273741666666667,0.020231016666666667,0.052173236666666664,0.030737785,0,0.033241405,0.019580376666666666);
/*!40000 ALTER TABLE `statssimulatortime` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `statstransformeranswers`
--

DROP TABLE IF EXISTS `statstransformeranswers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `statstransformeranswers` (
  `UserID` int NOT NULL,
  `P1M1A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P2M1A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P3M1A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P4M1A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P5M1A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P6M1A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P7M1A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P8M1A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P10M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P11M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P12M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P13M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P14M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P15M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P16M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P17M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P18M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P19M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P20M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P21M2A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P23M3A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P24M3A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P25M3A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P26M3A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P27M3A1` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P1M1A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P2M1A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P3M1A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P4M1A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P5M1A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P6M1A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P7M1A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P8M1A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P10M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P11M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P12M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P13M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P14M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P15M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P16M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P17M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P18M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P19M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P20M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P21M2A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P23M3A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P24M3A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P25M3A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P26M3A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  `P27M3A2` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '0',
  PRIMARY KEY (`UserID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `statstransformeranswers`
--

LOCK TABLES `statstransformeranswers` WRITE;
/*!40000 ALTER TABLE `statstransformeranswers` DISABLE KEYS */;
INSERT INTO `statstransformeranswers` VALUES (438910696,'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'),(750988303,'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'),(870167626,'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'),(1034515395,'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'),(1096972354,'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'),(1729203905,'5;6','4','4','6','6','5;6;7','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0');
/*!40000 ALTER TABLE `statstransformeranswers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `statstransformerbase`
--

DROP TABLE IF EXISTS `statstransformerbase`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `statstransformerbase` (
  `UserID` int NOT NULL,
  `StartCourseTime` datetime DEFAULT NULL,
  `EndCourseTime` datetime DEFAULT NULL,
  `AttemptsUsed` int NOT NULL DEFAULT '0',
  `RateAttempt1` double NOT NULL DEFAULT '0',
  `RateAttempt2` double NOT NULL DEFAULT '0',
  PRIMARY KEY (`UserID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `statstransformerbase`
--

LOCK TABLES `statstransformerbase` WRITE;
/*!40000 ALTER TABLE `statstransformerbase` DISABLE KEYS */;
INSERT INTO `statstransformerbase` VALUES (438910696,NULL,NULL,0,0,0),(750988303,NULL,NULL,0,0,0),(870167626,NULL,NULL,0,0,0),(1034515395,NULL,NULL,0,0,0),(1096972354,NULL,NULL,0,0,0),(1729203905,'2024-02-20 12:34:48',NULL,0,10,0);
/*!40000 ALTER TABLE `statstransformerbase` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `statstransformerrate`
--

DROP TABLE IF EXISTS `statstransformerrate`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `statstransformerrate` (
  `UserID` int NOT NULL,
  `P1M1A1` double NOT NULL DEFAULT '0',
  `P2M1A1` double NOT NULL DEFAULT '0',
  `P3M1A1` double NOT NULL DEFAULT '0',
  `P4M1A1` double NOT NULL DEFAULT '0',
  `P5M1A1` double NOT NULL DEFAULT '0',
  `P6M1A1` double NOT NULL DEFAULT '0',
  `P7M1A1` double NOT NULL DEFAULT '0',
  `P8M1A1` double NOT NULL DEFAULT '0',
  `P10M2A1` double NOT NULL DEFAULT '0',
  `P11M2A1` double NOT NULL DEFAULT '0',
  `P12M2A1` double NOT NULL DEFAULT '0',
  `P13M2A1` double NOT NULL DEFAULT '0',
  `P14M2A1` double NOT NULL DEFAULT '0',
  `P15M2A1` double NOT NULL DEFAULT '0',
  `P16M2A1` double NOT NULL DEFAULT '0',
  `P17M2A1` double NOT NULL DEFAULT '0',
  `P18M2A1` double NOT NULL DEFAULT '0',
  `P19M2A1` double NOT NULL DEFAULT '0',
  `P20M2A1` double NOT NULL DEFAULT '0',
  `P21M2A1` double NOT NULL DEFAULT '0',
  `P23M3A1` double NOT NULL DEFAULT '0',
  `P24M3A1` double NOT NULL DEFAULT '0',
  `P25M3A1` double NOT NULL DEFAULT '0',
  `P26M3A1` double NOT NULL DEFAULT '0',
  `P27M3A1` double NOT NULL DEFAULT '0',
  `P1M1A2` double NOT NULL DEFAULT '0',
  `P2M1A2` double NOT NULL DEFAULT '0',
  `P3M1A2` double NOT NULL DEFAULT '0',
  `P4M1A2` double NOT NULL DEFAULT '0',
  `P5M1A2` double NOT NULL DEFAULT '0',
  `P6M1A2` double NOT NULL DEFAULT '0',
  `P7M1A2` double NOT NULL DEFAULT '0',
  `P8M1A2` double NOT NULL DEFAULT '0',
  `P10M2A2` double NOT NULL DEFAULT '0',
  `P11M2A2` double NOT NULL DEFAULT '0',
  `P12M2A2` double NOT NULL DEFAULT '0',
  `P13M2A2` double NOT NULL DEFAULT '0',
  `P14M2A2` double NOT NULL DEFAULT '0',
  `P15M2A2` double NOT NULL DEFAULT '0',
  `P16M2A2` double NOT NULL DEFAULT '0',
  `P17M2A2` double NOT NULL DEFAULT '0',
  `P18M2A2` double NOT NULL DEFAULT '0',
  `P19M2A2` double NOT NULL DEFAULT '0',
  `P20M2A2` double NOT NULL DEFAULT '0',
  `P21M2A2` double NOT NULL DEFAULT '0',
  `P23M3A2` double NOT NULL DEFAULT '0',
  `P24M3A2` double NOT NULL DEFAULT '0',
  `P25M3A2` double NOT NULL DEFAULT '0',
  `P26M3A2` double NOT NULL DEFAULT '0',
  `P27M3A2` double NOT NULL DEFAULT '0',
  PRIMARY KEY (`UserID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `statstransformerrate`
--

LOCK TABLES `statstransformerrate` WRITE;
/*!40000 ALTER TABLE `statstransformerrate` DISABLE KEYS */;
INSERT INTO `statstransformerrate` VALUES (438910696,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(750988303,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(870167626,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(1034515395,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(1096972354,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(1729203905,-1,3,1.5,3,0,3.5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0);
/*!40000 ALTER TABLE `statstransformerrate` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `statstransformerstate`
--

DROP TABLE IF EXISTS `statstransformerstate`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `statstransformerstate` (
  `UserID` int NOT NULL,
  `Point` int NOT NULL DEFAULT '0',
  `ExtraAttempt` tinyint(1) DEFAULT NULL,
  `Attempts` int DEFAULT NULL,
  `Rate` double NOT NULL DEFAULT '0',
  `StartTime` datetime DEFAULT NULL,
  PRIMARY KEY (`UserID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `statstransformerstate`
--

LOCK TABLES `statstransformerstate` WRITE;
/*!40000 ALTER TABLE `statstransformerstate` DISABLE KEYS */;
INSERT INTO `statstransformerstate` VALUES (438910696,0,1,2,0,NULL),(750988303,0,1,2,0,NULL),(870167626,0,1,2,0,NULL),(1034515395,0,1,2,0,NULL),(1096972354,0,1,2,0,NULL),(1729203905,7,1,2,10,'2024-02-22 10:50:57');
/*!40000 ALTER TABLE `statstransformerstate` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `statstransformertime`
--

DROP TABLE IF EXISTS `statstransformertime`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `statstransformertime` (
  `UserID` int NOT NULL,
  `P1M1A1` double NOT NULL DEFAULT '0',
  `P2M1A1` double NOT NULL DEFAULT '0',
  `P3M1A1` double NOT NULL DEFAULT '0',
  `P4M1A1` double NOT NULL DEFAULT '0',
  `P5M1A1` double NOT NULL DEFAULT '0',
  `P6M1A1` double NOT NULL DEFAULT '0',
  `P7M1A1` double NOT NULL DEFAULT '0',
  `P8M1A1` double NOT NULL DEFAULT '0',
  `P10M2A1` double NOT NULL DEFAULT '0',
  `P11M2A1` double NOT NULL DEFAULT '0',
  `P12M2A1` double NOT NULL DEFAULT '0',
  `P13M2A1` double NOT NULL DEFAULT '0',
  `P14M2A1` double NOT NULL DEFAULT '0',
  `P15M2A1` double NOT NULL DEFAULT '0',
  `P16M2A1` double NOT NULL DEFAULT '0',
  `P17M2A1` double NOT NULL DEFAULT '0',
  `P18M2A1` double NOT NULL DEFAULT '0',
  `P19M2A1` double NOT NULL DEFAULT '0',
  `P20M2A1` double NOT NULL DEFAULT '0',
  `P21M2A1` double NOT NULL DEFAULT '0',
  `P23M3A1` double NOT NULL DEFAULT '0',
  `P24M3A1` double NOT NULL DEFAULT '0',
  `P25M3A1` double NOT NULL DEFAULT '0',
  `P26M3A1` double NOT NULL DEFAULT '0',
  `P27M3A1` double NOT NULL DEFAULT '0',
  `P1M1A2` double NOT NULL DEFAULT '0',
  `P2M1A2` double NOT NULL DEFAULT '0',
  `P3M1A2` double NOT NULL DEFAULT '0',
  `P4M1A2` double NOT NULL DEFAULT '0',
  `P5M1A2` double NOT NULL DEFAULT '0',
  `P6M1A2` double NOT NULL DEFAULT '0',
  `P7M1A2` double NOT NULL DEFAULT '0',
  `P8M1A2` double NOT NULL DEFAULT '0',
  `P10M2A2` double NOT NULL DEFAULT '0',
  `P11M2A2` double NOT NULL DEFAULT '0',
  `P12M2A2` double NOT NULL DEFAULT '0',
  `P13M2A2` double NOT NULL DEFAULT '0',
  `P14M2A2` double NOT NULL DEFAULT '0',
  `P15M2A2` double NOT NULL DEFAULT '0',
  `P16M2A2` double NOT NULL DEFAULT '0',
  `P17M2A2` double NOT NULL DEFAULT '0',
  `P18M2A2` double NOT NULL DEFAULT '0',
  `P19M2A2` double NOT NULL DEFAULT '0',
  `P20M2A2` double NOT NULL DEFAULT '0',
  `P21M2A2` double NOT NULL DEFAULT '0',
  `P23M3A2` double NOT NULL DEFAULT '0',
  `P24M3A2` double NOT NULL DEFAULT '0',
  `P25M3A2` double NOT NULL DEFAULT '0',
  `P26M3A2` double NOT NULL DEFAULT '0',
  `P27M3A2` double NOT NULL DEFAULT '0',
  PRIMARY KEY (`UserID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `statstransformertime`
--

LOCK TABLES `statstransformertime` WRITE;
/*!40000 ALTER TABLE `statstransformertime` DISABLE KEYS */;
INSERT INTO `statstransformertime` VALUES (438910696,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(750988303,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(870167626,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(1034515395,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(1096972354,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(1729203905,0.05439483666666667,0.028777231666666667,0.032286188333333334,0.046263123333333336,0.044124996666666666,0.08485176666666666,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0);
/*!40000 ALTER TABLE `statstransformertime` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `userflags`
--

DROP TABLE IF EXISTS `userflags`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `userflags` (
  `UserID` int NOT NULL,
  `StartDialogId` int DEFAULT NULL,
  `CurrentCourse` varchar(45) DEFAULT NULL,
  `ActivePollMessageId` int DEFAULT NULL,
  PRIMARY KEY (`UserID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `userflags`
--

LOCK TABLES `userflags` WRITE;
/*!40000 ALTER TABLE `userflags` DISABLE KEYS */;
INSERT INTO `userflags` VALUES (13513,0,NULL,0),(15645,0,NULL,0),(125135,0,NULL,0),(135131,0,NULL,0),(438910696,0,'simulator',7561),(750988303,0,NULL,0),(870167626,0,NULL,0),(1034515395,0,NULL,0),(1096972354,0,NULL,0),(1729203905,7600,NULL,7185);
/*!40000 ALTER TABLE `userflags` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `UserID` int NOT NULL,
  `Name` varchar(64) DEFAULT NULL,
  `Surname` varchar(64) DEFAULT NULL,
  `GroupNumber` varchar(64) DEFAULT NULL,
  PRIMARY KEY (`UserID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (13513,'Никита','Богданов','1111111-11111'),(15645,'Тимур','Карама','1111111-11111'),(125135,'Глеб','Жуков','1111111-11111'),(135131,'Максим','Приймак','1111111-11111'),(438910696,'Юзер','Гость',NULL),(750988303,'Ксения','Мануйлова','1111111-11111'),(870167626,'Анастасия','Швецова','1111111-11111'),(1034515395,'Анастасия','Новикова','1111111-11111'),(1096972354,'Екатерина','Попинова','1111111-11111'),(1729203905,'Илья','Свет','1111111-11111');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usersstate`
--

DROP TABLE IF EXISTS `usersstate`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usersstate` (
  `UserID` int NOT NULL,
  `DialogState` int DEFAULT NULL,
  `UserType` int DEFAULT NULL,
  `LogedIn` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`UserID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usersstate`
--

LOCK TABLES `usersstate` WRITE;
/*!40000 ALTER TABLE `usersstate` DISABLE KEYS */;
INSERT INTO `usersstate` VALUES (13513,0,2,0),(15645,0,2,0),(125135,0,2,0),(135131,0,2,0),(438910696,0,3,1),(750988303,0,0,0),(870167626,0,2,0),(1034515395,0,2,0),(1096972354,0,2,0),(1729203905,1,1,0);
/*!40000 ALTER TABLE `usersstate` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-02-22 17:44:09
