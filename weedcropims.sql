/*
SQLyog Ultimate v11.11 (64 bit)
MySQL - 5.0.22-community-nt : Database - weedcropims
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`weedcropims` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `weedcropims`;

/*Table structure for table `t_threatrategps` */

DROP TABLE IF EXISTS `t_threatrategps`;

CREATE TABLE `t_threatrategps` (
  `id` int(11) NOT NULL auto_increment,
  `imageNumber` varchar(255) default NULL,
  `latitude` varchar(255) default NULL,
  `longitude` varchar(255) default NULL,
  `tRate` float default NULL,
  PRIMARY KEY  (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `t_threatrategps` */

/*Table structure for table `t_weedcropsnum` */

DROP TABLE IF EXISTS `t_weedcropsnum`;

CREATE TABLE `t_weedcropsnum` (
  `id` int(11) NOT NULL auto_increment,
  `imageNumber` varchar(255) collate utf8_bin default NULL,
  `weedCount` int(11) default NULL,
  `weedDensity` float default NULL,
  `cropDensity` float default NULL,
  `soilDensity` float default NULL,
  `ciw` float default NULL,
  `cic` float default NULL,
  `cis` float default NULL,
  `aic` float default NULL,
  `tRate` float default NULL,
  PRIMARY KEY  (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

/*Data for the table `t_weedcropsnum` */

insert  into `t_weedcropsnum`(`id`,`imageNumber`,`weedCount`,`weedDensity`,`cropDensity`,`soilDensity`,`ciw`,`cic`,`cis`,`aic`,`tRate`) values (23,'wheatrow3',6,0.00242645,0.519447,0.633081,208,44528,54269,85722,0.00171396),(24,'wheatrow4',6,0.00242645,0.519447,0.633081,208,44528,54269,85722,0.00171396),(25,'wheatrow5',6,0.00242645,0.519447,0.633081,208,44528,54269,85722,0.00171396),(26,'wheatrow6',6,0.00242645,0.519447,0.633081,208,44528,54269,85722,0.00171396),(27,'wheatrow7',6,0.00242645,0.519447,0.633081,208,44528,54269,85722,0.00171396),(28,'wheatrow8',6,0.00242645,0.519447,0.633081,208,44528,54269,85722,0.00171396),(29,'wheatrow9',6,0.00242645,0.519447,0.633081,208,44528,54269,85722,0.00171396);

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
