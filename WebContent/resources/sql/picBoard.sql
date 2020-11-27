CREATE TABLE IF NOT EXISTS PicDB.picBoard(
	num int	not null auto_increment,
	id varchar(10) not null,
	name VARCHAR(50) not null,
	subject VARCHAR(100) not null,
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

select * from picBoard;

desc picBoard;

insert into picBoard values(1, "2", "3", "4", "5", "6", "7", "8", 9, "10", "11", "12", "13", "14", "15", 16);

insert into picBoard (id, name, subject, address, description, camera, filter, photoTime, category) values("ideal", "3", "4", "5", "6", "7", "8", 9, "10");

CREATE TABLE PicDB.board (
       num int not null auto_increment,
       id varchar(10) not null,
       name varchar(10) not null,
       subject varchar(100) not null,
       content text not null,
       regist_day varchar(30),
       hit int,
       ip varchar(20),
       PRIMARY KEY (num)
)default CHARSET=utf8; 