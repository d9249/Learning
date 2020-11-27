show databases;

CREATE TABLE IF NOT EXISTS BookMarketDB.book(
	b_id varchar(10) not null,
    b_name varchar(20),
    b_unitPrice integer,
    b_author varchar(50),
    b_publisher varchar(20),
    b_description text,
    b_category varchar(20),
    b_unitInStock long,
    b_totalPages long,
    b_releaseDate varchar(20),
    b_condition varchar(20),
    b_filename varchar(20),
    primary key (b_id)
)default charset=utf8;

desc BookMarketDB.book;

insert into BookMarketDB.book values('ISBN1234','HTML5+CSS3',15000,'황재호','한빛미디어','워드나 PPT 문서를 만들 수 있나요? 그러면 문제없습니다. 지금 바로 웹페이지 제작에 도전해보세요. 지금 당장 컴퓨터가 없어도 괜찮습니다. 코드와 실행 화면이 바로 보여서 눈으로만 읽어도 어떻게 작동하는지 쉽게 파악할 수 있는 것은 기본이고, 중간중간 퀴즈를 추가하여 재미있게 게임하듯 복습할 수 있습니다.','Hello Coding',1000,288,'2018/03/02','new','ISBN1234.jpg');
insert into BookMarketDB.book values('ISBN1235','쉽게 배우는 자바 프로그래밍',27000,'우종중','한빛아카데미','객체 지향의 핵심과 자바의 현대적 기능을 충실히 다루면서도 초보자가 쉽게 학습할 수 있게 구성했습니다. 시각화 도구를 활용한 개념 설명과 군더더기 없는 핵심 코드를 통해 개념과 구현을 한 흐름으로 학습할 수 있습니다. 또한 "기초 체력을 다지는 예제 -> 셀프 테스트 -> 생각을 논리적으로 정리하며 한 단계씩 풀어 가는 도전 과제 -> 스토리가 가미된 흥미로운 프로그래밍 문제" 등을 통해 프로그래밍 실력을 차근차근 끌어올릴 수 있습니다.','IT모바일',1000,692,'2017/08/02','new','ISBN1235.jpg');
insert into BookMarketDB.book values('ISBN1236','스프링4 입문',27000,'하세가와 유이치, 오오노 와타루, 토키 코헤이(권은철, 전민수)','한빛미디어','스프링은 단순히 사용 방법만 익히는 것보다 아키텍처를 어떻게 이해하고 설계하는지가 더 중요합니다. 예제를 복사해 붙여넣는 식으로는 실제 개발에서 스프링을 제대로 활용할 수 없습니다. 이 책에서는 웹 애플리케이션의 기초를 다지고 스프링 코어를 살펴보며 클라우드 네이티브 입문까지 다룹니다. 이제 막 실무에 뛰어든 웹 어플리케이션 초급자나 개발 경험은 있지만 스프링은 사용해본 적 없는 분을 대상으로 가능한 한 쉽게 설명합니다.','IT모바일',1000,520,'2017/11/01','new','ISBN1236.jpg');

select * from BookMarketDB.book;

desc BookMarketDB.book;

create table BookMarketDB.member (
	id varchar(10) not null,
	password varchar(10) not null,
	name varchar(10) not null,
	gender varchar(4),
	birth varchar(10),
	mail varchar(30),
	phone varchar(20),
	address varchar(90),
	regist_day varchar(50),
	primary key(id)
)default CHARSET = utf8;

desc BookMarketDB.member;

select * from BookMarketDB.member;

create table BookMarketDB.board (
	num int not null auto_increment,
	id varchar(10) not null,
	name varchar(10) not null,
	subject varchar(100) not null,
	content text not null,
	regist_day varchar(30),
	hit int,
	ip varchar(20),
	primary key(num)
)default CHARSET = utf8;

desc BookMarketDB.board;

select * from BookMarketDB.board;