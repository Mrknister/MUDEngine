-- phpMyAdmin SQL Dump
-- version 3.5.5
-- http://www.phpmyadmin.net
--
-- Host: localhost
-- Generation Time: Jan 12, 2013 at 11:57 AM
-- Server version: 5.5.28-log
-- PHP Version: 5.4.9

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Database: `MUDEngine`
--

-- --------------------------------------------------------

--
-- Table structure for table `Armor`
--

CREATE TABLE IF NOT EXISTS `Armor` (
  `I_Id` int(11) NOT NULL,
  `PhyRes` int(11) NOT NULL,
  `MaRes` int(11) NOT NULL,
  `Type` enum('Head','Chest','Legs','Foot','Hands','Wrist','Waist','Amulet','Ring','Ring2','Shield') NOT NULL,
  UNIQUE KEY `I_Id` (`I_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `Armor`
--

INSERT INTO `Armor` (`I_Id`, `PhyRes`, `MaRes`, `Type`) VALUES
(2, 20, 10, 'Chest'),
(4, 10, 5, 'Head');

-- --------------------------------------------------------

--
-- Table structure for table `BelongsTo`
--

CREATE TABLE IF NOT EXISTS `BelongsTo` (
  `C_Id` int(11) NOT NULL,
  `I_Id` int(11) NOT NULL,
  `Amount` int(11) NOT NULL,
  `Equipped` tinyint(1) NOT NULL,
  UNIQUE KEY `C_Id` (`C_Id`,`I_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `BelongsTo`
--

INSERT INTO `BelongsTo` (`C_Id`, `I_Id`, `Amount`, `Equipped`) VALUES
(1, 1, 2, 1),
(1, 2, 1, 1),
(1, 3, 4, 0),
(1, 5, 10, 0),
(1, 7, 10, 0);

-- --------------------------------------------------------

--
-- Table structure for table `Buff`
--

CREATE TABLE IF NOT EXISTS `Buff` (
  `B_Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(16) NOT NULL,
  `Amount` int(11) NOT NULL,
  `Type` int(11) NOT NULL,
  `RunsOutAt` datetime NOT NULL,
  `C_Id` int(11) NOT NULL,
  PRIMARY KEY (`B_Id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=2 ;

--
-- Dumping data for table `Buff`
--

INSERT INTO `Buff` (`B_Id`, `Name`, `Amount`, `Type`, `RunsOutAt`, `C_Id`) VALUES
(1, 'Steroide des Unt', 50, 2, '2013-01-11 23:51:23', 1);

-- --------------------------------------------------------

--
-- Table structure for table `Character`
--

CREATE TABLE IF NOT EXISTS `Character` (
  `C_Id` int(11) NOT NULL AUTO_INCREMENT,
  `U_Id` int(11) NOT NULL,
  `R_Id` int(11) NOT NULL,
  `Money` int(11) NOT NULL,
  `Health` int(11) NOT NULL,
  `Damage` int(11) NOT NULL,
  `PhRes` int(11) NOT NULL,
  `MaxHealth` int(11) NOT NULL,
  `Name` varchar(16) NOT NULL,
  PRIMARY KEY (`C_Id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=18 ;

--
-- Dumping data for table `Character`
--

INSERT INTO `Character` (`C_Id`, `U_Id`, `R_Id`, `Money`, `Health`, `Damage`, `PhRes`, `MaxHealth`, `Name`) VALUES
(1, 1, 1, 10000, 40, 10, 0, 100, 'Ladislaus'),
(9, 3, 1, 200, 100, 10, 10, 100, 'Meenhard'),
(17, 1, 1, 200, 100, 10, 10, 100, 'Legolas');

-- --------------------------------------------------------

--
-- Table structure for table `Consumable`
--

CREATE TABLE IF NOT EXISTS `Consumable` (
  `I_Id` int(11) NOT NULL,
  `Duration` time NOT NULL,
  `Type` enum('Health','PhRes','Damage','Food','Beverage') NOT NULL,
  `Amount` int(11) NOT NULL,
  `ConsumptionType` tinyint(1) NOT NULL DEFAULT '1' COMMENT 'True for drinkable false for eatable',
  UNIQUE KEY `I_Id` (`I_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `Consumable`
--

INSERT INTO `Consumable` (`I_Id`, `Duration`, `Type`, `Amount`, `ConsumptionType`) VALUES
(3, '00:06:00', 'Health', 10, 0),
(5, '00:06:00', 'PhRes', 50, 1),
(7, '00:10:00', 'Damage', 50, 1);

-- --------------------------------------------------------

--
-- Table structure for table `Gate`
--

CREATE TABLE IF NOT EXISTS `Gate` (
  `R_IdF` int(11) NOT NULL,
  `R_IdT` int(11) NOT NULL,
  `Direction` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `Gate`
--

INSERT INTO `Gate` (`R_IdF`, `R_IdT`, `Direction`) VALUES
(1, 2, 'North'),
(2, 1, 'South'),
(2, 3, 'North'),
(3, 2, 'South'),
(2, 4, 'West'),
(4, 2, 'East'),
(2, 5, 'East'),
(5, 2, 'West'),
(2, 6, 'Up'),
(6, 2, 'Down'),
(2, 7, 'Down'),
(7, 2, 'Up');

-- --------------------------------------------------------

--
-- Table structure for table `Item`
--

CREATE TABLE IF NOT EXISTS `Item` (
  `I_Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(64) NOT NULL,
  `Weight` int(11) NOT NULL,
  `Value` int(11) NOT NULL,
  `Category` enum('Armor','Weapon','Consumable','Note') NOT NULL,
  `Description` varchar(255) NOT NULL,
  PRIMARY KEY (`I_Id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=18 ;

--
-- Dumping data for table `Item`
--

INSERT INTO `Item` (`I_Id`, `Name`, `Weight`, `Value`, `Category`, `Description`) VALUES
(1, 'Gladius', 300, 1000, 'Weapon', 'Nachbildung eines röm. Gladius.'),
(2, 'Kettenhemd', 2000, 4000, 'Armor', 'Ein Kettenhemd'),
(3, 'Keks', 100, 1, 'Consumable', 'Lecker !'),
(4, 'Helm', 100, 200, 'Armor', 'Ein Römerhelm'),
(5, 'Trank der Steinhaut', 1, 200, 'Consumable', 'Laesst eure Haut so resistent wie Stein werden und erhöht damit euren Rüstungswert.'),
(7, 'Steroide des Untergangs', 2, 5000, 'Consumable', 'Das Gift ist so Potent, dass sogar langsam die Phiole zersetzt.'),
(8, 'Stachelbesetze Schlachtruestung', 120, 8000, 'Armor', 'Eine mit Stahldornenbesetzte Plattenruestung mit dem Emblem des Meisterschmiedes Eldra.'),
(9, 'Zerfetztes loechriges Leinenhemd', 5, 10, 'Armor', 'Dieses Hemd bietet nur Schutz vor Wind und Wetter jedoch nicht  vor Angriffen.'),
(10, 'Nietenbeschlagende Lederruestung', 40, 2000, 'Armor', 'Stabile Ruestung aus Hirschleder mit gutem Schutz vor Hieben und Stichen'),
(11, 'Schwarzer Schlachtpanzer ', 250, 100000, 'Armor', 'Diese Ruestung geschmiedet in den dunkelsten Feuern der Schwarzstahlwueste.'),
(12, 'Verstaerkte Lederruestung der Kel''Magan', 20, 7000, 'Armor', 'Hervorragende Lederruestung  der Kel''Magan, welche im inneren mit Trollleder verstaerkt wurde und somit vor Kaelte schuetzt.'),
(13, 'Umhang der schwarzen Schatten', 5, 200000, 'Armor', 'Den Umhang der schwarzen Schatten verschwimmt mit seiner Umgebung und macht es den Gegner schwer euch Schaden zuzufügen.'),
(14, 'Blutige Ruestung des Schlaechters', 150, 6000, 'Armor', 'Diese verfluchte Ruestung wiegt schwer durch das Blut der Opfer des Schlaechters. '),
(15, 'Alte Ruestung des einsamen Helden', 110, 20000, 'Armor', 'Vor langer Zeit erstrahlte diese Ruestung in gleißendem Licht, von dem nun nur noch ein  kleiner Funke vorhanden ist .'),
(16, 'Lavageschmiedete  Ruestung des Feuerlords', 20, 250000, 'Armor', 'Trotz ihrer  Stabilität ist die Rüstung erstaunlich leicht, und ist angenehm kühl.'),
(17, 'Glaenzende Ruestung des Groszvasalls', 85, 10000, 'Armor', 'Diese Ruestung gehoerte einst dem Groszvasall bevor er von einem Orc niedergestreckt wurde');

-- --------------------------------------------------------

--
-- Table structure for table `LearnedSpells`
--

CREATE TABLE IF NOT EXISTS `LearnedSpells` (
  `C_Id` int(11) NOT NULL,
  `S_Id` int(11) NOT NULL,
  `Rank` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `Loot`
--

CREATE TABLE IF NOT EXISTS `Loot` (
  `I_Id` int(11) NOT NULL,
  `M_Id` int(11) NOT NULL,
  `Probability` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `Loot`
--

INSERT INTO `Loot` (`I_Id`, `M_Id`, `Probability`) VALUES
(3, 1, 1);

-- --------------------------------------------------------

--
-- Table structure for table `Monster`
--

CREATE TABLE IF NOT EXISTS `Monster` (
  `M_Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(32) NOT NULL,
  `Description` varchar(255) NOT NULL,
  `Hostile` tinyint(1) NOT NULL,
  `Damage` int(11) NOT NULL,
  `MaxHealth` int(11) NOT NULL,
  PRIMARY KEY (`M_Id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=2 ;

--
-- Dumping data for table `Monster`
--

INSERT INTO `Monster` (`M_Id`, `Name`, `Description`, `Hostile`, `Damage`, `MaxHealth`) VALUES
(1, 'Troll', 'Trollolololololo trololololo hohohoho...', 1, 40, 100);

-- --------------------------------------------------------

--
-- Table structure for table `MonsterIsIn`
--

CREATE TABLE IF NOT EXISTS `MonsterIsIn` (
  `M_Id` int(11) NOT NULL,
  `R_Id` int(11) NOT NULL,
  `Respawntime` time NOT NULL,
  `RespawnAtTime` datetime NOT NULL,
  `Health` int(11) NOT NULL,
  `Mana` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `MonsterIsIn`
--

INSERT INTO `MonsterIsIn` (`M_Id`, `R_Id`, `Respawntime`, `RespawnAtTime`, `Health`, `Mana`) VALUES
(1, 1, '00:05:00', '2013-01-11 22:36:54', 40, 0);

-- --------------------------------------------------------

--
-- Table structure for table `Note`
--

CREATE TABLE IF NOT EXISTS `Note` (
  `I_Id` int(11) NOT NULL,
  `Text` text NOT NULL,
  UNIQUE KEY `I_Id` (`I_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `Objekt`
--

CREATE TABLE IF NOT EXISTS `Objekt` (
  `O_Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(16) NOT NULL,
  `Description` varchar(255) NOT NULL,
  PRIMARY KEY (`O_Id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=4 ;

--
-- Dumping data for table `Objekt`
--

INSERT INTO `Objekt` (`O_Id`, `Name`, `Description`) VALUES
(1, 'Gladius', 'Nachbildung eines röm. Gladius.'),
(2, 'Truhe', 'In der Truhe befindet sich ein Gladius und ein Helm.'),
(3, 'Helm', 'Ein Römerhelm');

-- --------------------------------------------------------

--
-- Table structure for table `ObjInRoom`
--

CREATE TABLE IF NOT EXISTS `ObjInRoom` (
  `R_Id` int(11) NOT NULL,
  `O_Id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `ObjInRoom`
--

INSERT INTO `ObjInRoom` (`R_Id`, `O_Id`) VALUES
(2, 1),
(2, 2),
(2, 3);

-- --------------------------------------------------------

--
-- Table structure for table `Room`
--

CREATE TABLE IF NOT EXISTS `Room` (
  `R_Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(16) NOT NULL,
  `Description` varchar(255) NOT NULL,
  PRIMARY KEY (`R_Id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=8 ;

--
-- Dumping data for table `Room`
--

INSERT INTO `Room` (`R_Id`, `Name`, `Description`) VALUES
(1, 'Empfangshalle', 'Willkommen !'),
(2, 'Zentrum', 'Von hier aus geht es in alle Nebenräume.In der Raummitte liegt ein Gladius in einer offenen Vitrine'),
(3, 'Nordflügel(A)', 'Der Nördliche Teil des Gebäudes - Nach Süden geht es zurück zur Zentrale. - Nach Norden geht es in die weite Welt hinaus! (gesperrt)'),
(4, 'Westflügel', 'Der westliche Teil des gebäudes. - Nach Osten geht es zurück ins Zentrum.'),
(5, 'Ostflügel', 'Der östliche Teil des Gebäudes. - Nach Westen geht es zur Zentrale.'),
(6, 'Dach', 'Eine schöne Aussicht ! - Runter geht es zurück ins Zentrum.'),
(7, 'Keller', 'Hier lagert Wein und Käse. - Hoch geht es zurück ins Zentrum.\nIn einer dunklen Ecke hock ein Troll und frisst Käse. Er scheint ziemlich beschäftigt ,also störe ihn lieber nicht !');

-- --------------------------------------------------------

--
-- Table structure for table `Takeable`
--

CREATE TABLE IF NOT EXISTS `Takeable` (
  `I_Id` int(11) NOT NULL,
  `O_Id` int(11) NOT NULL,
  `Respawntime` time NOT NULL,
  `RespawnAtTime` datetime NOT NULL,
  `Takefrom` varchar(64) NOT NULL,
  `R_Id` int(11) NOT NULL,
  UNIQUE KEY `I_Id` (`I_Id`,`O_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `Takeable`
--

INSERT INTO `Takeable` (`I_Id`, `O_Id`, `Respawntime`, `RespawnAtTime`, `Takefrom`, `R_Id`) VALUES
(1, 1, '00:05:00', '2013-01-04 00:15:00', 'Truhe', 2),
(4, 3, '00:05:00', '2013-01-04 00:15:00', 'Truhe', 2);

-- --------------------------------------------------------

--
-- Table structure for table `User`
--

CREATE TABLE IF NOT EXISTS `User` (
  `U_Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(16) NOT NULL,
  `Password` int(16) NOT NULL,
  `LastLogin` datetime NOT NULL,
  PRIMARY KEY (`U_Id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=7 ;

--
-- Dumping data for table `User`
--

INSERT INTO `User` (`U_Id`, `Name`, `Password`, `LastLogin`) VALUES
(1, 'Julian', 14789632, '2013-01-03 21:02:54'),
(3, 'Jan', 123789456, '2013-01-03 23:58:47');

-- --------------------------------------------------------

--
-- Table structure for table `Weapon`
--

CREATE TABLE IF NOT EXISTS `Weapon` (
  `I_Id` int(11) NOT NULL,
  `Damage` int(11) NOT NULL,
  `Type` enum('Sword','Dagger','Axe','Staff','2HandSword','2HandAxe','Mace','Hammer','Bow','Crossbow') NOT NULL,
  UNIQUE KEY `I_Id` (`I_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `Weapon`
--

INSERT INTO `Weapon` (`I_Id`, `Damage`, `Type`) VALUES
(1, 20, 'Sword');
