CREATE TABLE IF NOT EXISTS PicDB.pic(
	p_name VARCHAR(50) NOT NULL,
	p_address VARCHAR(50),
	p_camera VARCHAR(30),
	p_filter VARCHAR(30),
	p_description TEXT,
   	p_time VARCHAR(30),
	p_category VARCHAR(20),
	p_fileName  VARCHAR(20),
	PRIMARY KEY (p_name)
)default CHARSET=utf8;

desc PicDB.pic;