

DROP TABLE IF EXISTS `Armor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Armor` (
  `I_Id` int(11) NOT NULL,
  `PhyRes` int(11) NOT NULL,
  `MaRes` int(11) NOT NULL,
  `Type` enum('Head','Chest','Legs','Foot','Hands','Wrist','Waist','Amulet','Ring','Ring2','Shield') NOT NULL,
  UNIQUE KEY `I_Id` (`I_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;


LOCK TABLES `Armor` WRITE;
/*!40000 ALTER TABLE `Armor` DISABLE KEYS */;
/*!40000 ALTER TABLE `Armor` ENABLE KEYS */;
UNLOCK TABLES;


DROP TABLE IF EXISTS `BelongsTo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `BelongsTo` (
  `C_Id` int(11) NOT NULL,
  `I_Id` int(11) NOT NULL,
  `Amount` int(11) NOT NULL,
  `Equipped` tinyint(1) NOT NULL,
  UNIQUE KEY `C_Id` (`C_Id`,`I_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;



LOCK TABLES `BelongsTo` WRITE;
/*!40000 ALTER TABLE `BelongsTo` DISABLE KEYS */;
/*!40000 ALTER TABLE `BelongsTo` ENABLE KEYS */;
UNLOCK TABLES;



DROP TABLE IF EXISTS `Buff`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Buff` (
  `B_Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(16) NOT NULL,
  `Amount` int(11) NOT NULL,
  `Type` int(11) NOT NULL,
  `RunsOutAt` datetime NOT NULL,
  `C_Id` int(11) NOT NULL,
  PRIMARY KEY (`B_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;


LOCK TABLES `Buff` WRITE;
/*!40000 ALTER TABLE `Buff` DISABLE KEYS */;
/*!40000 ALTER TABLE `Buff` ENABLE KEYS */;
UNLOCK TABLES;


DROP TABLE IF EXISTS `Character`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Character` (
  `C_Id` int(11) NOT NULL AUTO_INCREMENT,
  `U_Id` int(11) NOT NULL,
  `R_Id` int(11) NOT NULL,
  `Money` int(11) NOT NULL,
  `Health` int(11) NOT NULL,
  `Mana` int(11) NOT NULL,
  `Damage` int(11) NOT NULL,
  `PhRes` int(11) NOT NULL,
  `MaRes` int(11) NOT NULL,
  `MaxHealth` int(11) NOT NULL,
  `MaxMana` int(11) NOT NULL,
  `Name` varchar(16) NOT NULL,
  PRIMARY KEY (`C_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;



LOCK TABLES `Character` WRITE;
/*!40000 ALTER TABLE `Character` DISABLE KEYS */;
/*!40000 ALTER TABLE `Character` ENABLE KEYS */;
UNLOCK TABLES;



DROP TABLE IF EXISTS `Consumable`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Consumable` (
  `I_Id` int(11) NOT NULL,
  `Duration` time NOT NULL,
  `Type` enum('Health','Mana','Armor','MaRes','Poison','Food','Breverage') NOT NULL,
  `Amount` int(11) NOT NULL,
  UNIQUE KEY `I_Id` (`I_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;


LOCK TABLES `Consumable` WRITE;
/*!40000 ALTER TABLE `Consumable` DISABLE KEYS */;
/*!40000 ALTER TABLE `Consumable` ENABLE KEYS */;
UNLOCK TABLES;


DROP TABLE IF EXISTS `Gate`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Gate` (
  `R_IdF` int(11) NOT NULL,
  `R_IdT` int(11) NOT NULL,
  `Direction` enum('North','South','East','West','Up','Down') NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;



LOCK TABLES `Gate` WRITE;
/*!40000 ALTER TABLE `Gate` DISABLE KEYS */;
/*!40000 ALTER TABLE `Gate` ENABLE KEYS */;
UNLOCK TABLES;



DROP TABLE IF EXISTS `Item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Item` (
  `I_Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(16) NOT NULL,
  `Weight` int(11) NOT NULL,
  `Value` int(11) NOT NULL,
  `Category` enum('Armor','Weapon','Consumable') NOT NULL,
  `Description` varchar(255) NOT NULL,
  PRIMARY KEY (`I_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;



LOCK TABLES `Item` WRITE;
/*!40000 ALTER TABLE `Item` DISABLE KEYS */;
/*!40000 ALTER TABLE `Item` ENABLE KEYS */;
UNLOCK TABLES;


DROP TABLE IF EXISTS `LearnedSpells`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `LearnedSpells` (
  `C_Id` int(11) NOT NULL,
  `S_Id` int(11) NOT NULL,
  `Rank` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;



LOCK TABLES `LearnedSpells` WRITE;
/*!40000 ALTER TABLE `LearnedSpells` DISABLE KEYS */;
/*!40000 ALTER TABLE `LearnedSpells` ENABLE KEYS */;
UNLOCK TABLES;


DROP TABLE IF EXISTS `Loot`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Loot` (
  `I_Id` int(11) NOT NULL,
  `M_Id` int(11) NOT NULL,
  `Probability` float NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;



LOCK TABLES `Loot` WRITE;
/*!40000 ALTER TABLE `Loot` DISABLE KEYS */;
/*!40000 ALTER TABLE `Loot` ENABLE KEYS */;
UNLOCK TABLES;



DROP TABLE IF EXISTS `Monster`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Monster` (
  `M_Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(32) NOT NULL,
  `Description` varchar(255) NOT NULL,
  `Hostile` tinyint(1) NOT NULL,
  `Damage` int(11) NOT NULL,
  `MaxMana` int(11) NOT NULL,
  `MaxHealth` int(11) NOT NULL,
  PRIMARY KEY (`M_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;



LOCK TABLES `Monster` WRITE;
/*!40000 ALTER TABLE `Monster` DISABLE KEYS */;
/*!40000 ALTER TABLE `Monster` ENABLE KEYS */;
UNLOCK TABLES;



DROP TABLE IF EXISTS `MonsterAttackMsg`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `MonsterAttackMsg` (
  `Msg_Id` int(11) NOT NULL AUTO_INCREMENT,
  `Text` text NOT NULL,
  `Type` enum('Dodge','Miss','Hit','CriticalHit','Parried','WeakHit','Default') NOT NULL,
  `M_Id` int(11) NOT NULL,
  PRIMARY KEY (`Msg_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;



LOCK TABLES `MonsterAttackMsg` WRITE;
/*!40000 ALTER TABLE `MonsterAttackMsg` DISABLE KEYS */;
/*!40000 ALTER TABLE `MonsterAttackMsg` ENABLE KEYS */;
UNLOCK TABLES;

DROP TABLE IF EXISTS `MonsterIsIn`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `MonsterIsIn` (
  `M_Id` int(11) NOT NULL,
  `R_Id` int(11) NOT NULL,
  `Respawntime` time NOT NULL,
  `RespawnAtTime` datetime NOT NULL,
  `Health` datetime NOT NULL,
  `Mana` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;


LOCK TABLES `MonsterIsIn` WRITE;
/*!40000 ALTER TABLE `MonsterIsIn` DISABLE KEYS */;
/*!40000 ALTER TABLE `MonsterIsIn` ENABLE KEYS */;
UNLOCK TABLES;



DROP TABLE IF EXISTS `MonsterSpellsLearned`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `MonsterSpellsLearned` (
  `S_Id` int(11) NOT NULL,
  `M_Id` int(11) NOT NULL,
  `U` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;



LOCK TABLES `MonsterSpellsLearned` WRITE;
/*!40000 ALTER TABLE `MonsterSpellsLearned` DISABLE KEYS */;
/*!40000 ALTER TABLE `MonsterSpellsLearned` ENABLE KEYS */;
UNLOCK TABLES;



DROP TABLE IF EXISTS `Note`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Note` (
  `I_Id` int(11) NOT NULL,
  `Text` text NOT NULL,
  UNIQUE KEY `I_Id` (`I_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;



LOCK TABLES `Note` WRITE;
/*!40000 ALTER TABLE `Note` DISABLE KEYS */;
/*!40000 ALTER TABLE `Note` ENABLE KEYS */;
UNLOCK TABLES;



DROP TABLE IF EXISTS `ObjInRoom`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ObjInRoom` (
  `R_Id` int(11) NOT NULL,
  `O_Id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;



LOCK TABLES `ObjInRoom` WRITE;
/*!40000 ALTER TABLE `ObjInRoom` DISABLE KEYS */;
/*!40000 ALTER TABLE `ObjInRoom` ENABLE KEYS */;
UNLOCK TABLES;



DROP TABLE IF EXISTS `Objekt`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Objekt` (
  `O_Id` int(11) NOT NULL,
  `Name` varchar(16) NOT NULL,
  `Description` varchar(255) NOT NULL,
  PRIMARY KEY (`O_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;



LOCK TABLES `Objekt` WRITE;
/*!40000 ALTER TABLE `Objekt` DISABLE KEYS */;
/*!40000 ALTER TABLE `Objekt` ENABLE KEYS */;
UNLOCK TABLES;


DROP TABLE IF EXISTS `Room`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Room` (
  `R_Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(16) NOT NULL,
  `Description` varchar(255) NOT NULL,
  PRIMARY KEY (`R_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;



LOCK TABLES `Room` WRITE;
/*!40000 ALTER TABLE `Room` DISABLE KEYS */;
/*!40000 ALTER TABLE `Room` ENABLE KEYS */;
UNLOCK TABLES;


DROP TABLE IF EXISTS `Spells`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Spells` (
  `S_Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(16) NOT NULL,
  `Cooldown` time NOT NULL,
  `Cost` int(11) NOT NULL,
  PRIMARY KEY (`S_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;



LOCK TABLES `Spells` WRITE;
/*!40000 ALTER TABLE `Spells` DISABLE KEYS */;
/*!40000 ALTER TABLE `Spells` ENABLE KEYS */;
UNLOCK TABLES;



DROP TABLE IF EXISTS `Takeable`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Takeable` (
  `I_Id` int(11) NOT NULL,
  `O_Id` int(11) NOT NULL,
  `Respawntime` time NOT NULL,
  `RespawnAtTime` datetime NOT NULL,
  `Takefrom` varchar(64) NOT NULL,
  UNIQUE KEY `I_Id` (`I_Id`,`O_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;



LOCK TABLES `Takeable` WRITE;
/*!40000 ALTER TABLE `Takeable` DISABLE KEYS */;
/*!40000 ALTER TABLE `Takeable` ENABLE KEYS */;
UNLOCK TABLES;



DROP TABLE IF EXISTS `User`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `User` (
  `U_Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(16) NOT NULL,
  `Password` int(16) NOT NULL,
  `LastLogin` datetime NOT NULL,
  PRIMARY KEY (`U_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;



LOCK TABLES `User` WRITE;
/*!40000 ALTER TABLE `User` DISABLE KEYS */;
/*!40000 ALTER TABLE `User` ENABLE KEYS */;
UNLOCK TABLES;


DROP TABLE IF EXISTS `Weapon`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Weapon` (
  `I_Id` int(11) NOT NULL,
  `Damage` int(11) NOT NULL,
  `Type` enum('Sword','Dagger','Axe','Staff','2HandSword','2HandAxe','Mace','Hammer','Bow','Crossbow') NOT NULL,
  UNIQUE KEY `I_Id` (`I_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;



LOCK TABLES `Weapon` WRITE;
/*!40000 ALTER TABLE `Weapon` DISABLE KEYS */;
/*!40000 ALTER TABLE `Weapon` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

