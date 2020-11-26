INSERT INTO product VALUES('P1234', 'iPhone 6s', 800000, '1334X750 Renina HD display, 8-megapixel iSight Camera','Smart Phone', 'Apple', 1000, 'new', 'P1234.png');
INSERT INTO product VALUES('P1235', 'LG PC gram', 1500000, '3.3-inch,IPS LED display, 5rd Generation Intel Core processors', 'Notebook', 'LG', 1000, 'new', 'P1235.png');
INSERT INTO product VALUES('P1236', 'Galaxy Tab S', 900000, '3.3-inch, 212.8*125.6*6.6mm,  Super AMOLED display, Octa-Core processor', 'Tablet', 'Samsung', 1000, 'new', 'P1236.png');

select * from product;

insert into pic values('오늘의 날씨', '대부도 어딘가', '아이폰SE', '스노우', '오랜만에 대부도에서 한장', '저녁 6시쯤', '풍경', 'P1234.jpg');

desc PicDB.pic;

select * from pic;

Pic one = new Pic("오늘의 날씨", "대부도 어딘가", "아이폰SE");
		one.setFilter("스노우");
		one.setDescription("오랜만에 대부도에서 한장");
		one.setTime("저녁 6시쯤");
		one.setCategory("풍경");
		one.setFilename("P1234.png");