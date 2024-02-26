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
INSERT INTO `courses` VALUES ('simulator');
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
INSERT INTO `groupscourses` VALUES ('1111111-11111','simulator');
/*!40000 ALTER TABLE `groupscourses` ENABLE KEYS */;
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
INSERT INTO `statssimulatoranswers` VALUES (13513,'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'),(15645,'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'),(125135,'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'),(135131,'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'),(438910696,'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'),(750988303,'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'),(870167626,'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'),(1034515395,'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'),(1096972354,'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'),(1729203905,'7','4','5','6','6','5','0','3','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0');
/*!40000 ALTER TABLE `statssimulatoranswers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `statssimulatorbased`
--

DROP TABLE IF EXISTS `statssimulatorbased`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `statssimulatorbased` (
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
-- Dumping data for table `statssimulatorbased`
--

LOCK TABLES `statssimulatorbased` WRITE;
/*!40000 ALTER TABLE `statssimulatorbased` DISABLE KEYS */;
INSERT INTO `statssimulatorbased` VALUES (13513,NULL,NULL,0,0,0),(15645,NULL,NULL,0,0,0),(125135,NULL,NULL,0,0,0),(135131,NULL,NULL,0,0,0),(438910696,NULL,NULL,0,0,0),(750988303,NULL,NULL,0,0,0),(870167626,NULL,NULL,0,0,0),(1034515395,NULL,NULL,0,0,0),(1096972354,NULL,NULL,0,0,0),(1729203905,'2024-02-26 13:41:26',NULL,0,11.5,0);
/*!40000 ALTER TABLE `statssimulatorbased` ENABLE KEYS */;
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
INSERT INTO `statssimulatorrate` VALUES (13513,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(15645,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(125135,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(135131,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(438910696,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(750988303,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(870167626,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(1034515395,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(1096972354,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(1729203905,0,3,0,3,0,1,4,0.5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0);
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
INSERT INTO `statssimulatorstate` VALUES (13513,0,1,2,0,NULL),(15645,0,1,2,0,NULL),(125135,0,1,2,0,NULL),(135131,0,1,2,0,NULL),(438910696,0,1,2,0,NULL),(750988303,0,1,2,0,NULL),(870167626,0,1,2,0,NULL),(1034515395,0,1,2,0,NULL),(1096972354,0,1,2,0,NULL),(1729203905,0,0,2,0,'2024-02-26 13:42:11');
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
INSERT INTO `statssimulatortime` VALUES (13513,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(15645,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(125135,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(135131,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(438910696,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(750988303,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(870167626,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(1034515395,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(1096972354,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(1729203905,0.041707611666666665,0.02541373,0.022919088333333334,0.03593672666666667,0.02898376,0.04072825,0.5970335533333333,0.04719343833333333,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0);
/*!40000 ALTER TABLE `statssimulatortime` ENABLE KEYS */;
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
INSERT INTO `userflags` VALUES (13513,0,NULL,0),(15645,0,NULL,0),(125135,0,NULL,0),(135131,0,NULL,0),(438910696,0,NULL,7561),(750988303,0,NULL,0),(870167626,0,NULL,0),(1034515395,0,NULL,0),(1096972354,0,NULL,0),(1729203905,0,NULL,8614);
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
INSERT INTO `users` VALUES (13513,'Никита','Богданов','1111111-11111'),(15645,'Тимур','Карама','1111111-11111'),(125135,'Глеб','Жуков','1111111-11111'),(135131,'Максим','Приймак','1111111-11111'),(438910696,'Юзер','Гость',NULL),(750988303,'Ксения','Мануйлова','1111111-11111'),(870167626,'Анастасия','Швецова','1111111-11111'),(1034515395,'Анастасия','Новикова','1111111-11111'),(1096972354,'Екатерина','Попинова','1111111-11111'),(1729203905,'Юзер','Гость',NULL);
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
INSERT INTO `usersstate` VALUES (13513,0,2,0),(15645,0,2,0),(125135,0,2,0),(135131,0,2,0),(438910696,0,3,1),(750988303,0,0,0),(870167626,0,2,0),(1034515395,0,2,0),(1096972354,0,2,0),(1729203905,0,3,1);
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

-- Dump completed on 2024-02-26 13:49:50
