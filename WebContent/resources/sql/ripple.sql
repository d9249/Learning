create table rippleBoard (
	num int not null auto_increment,
	parent int not null,
	id varchar(10),
	name varchar(10),
	content text not null,
	regist_day varchar(30),
	ip varchar(20),
	PRIMARY KEY (num)
)default CHARSET=utf8;


CREATE TABLE IF NOT EXISTS PicDB.picBoard(
	num int	not null auto_increment,
	id varchar(10) not null.
	name VARCHAR(50) NOT NULL,
	address VARCHAR(50) not null,
	description TEXT not null,
	camera VARCHAR(30),
	filter VARCHAR(30),
	hit int,
   	photoTime VARCHAR(30),
	category VARCHAR(20),
	ip varchar(20),
	regist_day varchar(30),
	filename  VARCHAR(20),
	filesize long,
	PRIMARY KEY (num)
)default CHARSET=utf8;