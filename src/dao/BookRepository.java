package dao;

import java.util.ArrayList;

import dto.Book;

public class BookRepository {
	
	private ArrayList<Book> listOfBooks = new ArrayList<Book>();
	private static BookRepository instance = new BookRepository();
	
	public static BookRepository getInstance() {
		return instance;
	}
	
	public BookRepository() {
		Book one = new Book();
		one.setDescription("워드나 PPT 문서를 만들 수 있나요? 그러면 문제 없습니다. 지금 바로 웹페이지 제작에 도전해보세요."
				+ " 지금 당장 컴퓨터가 없어도 괜찮습니다. 코드와 실행 화면이 바로 보여서 눈으로만 읽어도 어떻게 작동하는지 쉽게 파악할 수 있는 것은 기본이고,"
				+ "중간중간 퀴즈를 추가하여 재미있게 게임하듯 복습할 수 있습니다.");
		one.setBookId("ISBN1234");
		one.setUnitInStock(1000);
		one.setTotalPages(288);
		one.setReleaseDate("2018/03/02");
		one.setName("HTML5+CSS3");
		one.setCategory("Hello Coding");
		one.setAuthor("황재호");
		one.setPublisher("한빛미디어");
		one.setUnitPrice(15000);
		one.setCondition("New");
		one.setFilename("ISBN1234.jpg");
	
		Book two = new Book();
		two.setDescription("객체 지향의 핵심과 자바의 현대적 기능을 충실히 다루면서도 초보자가 쉽게 학습할 수 있게 구성했습니다."
				+ "시각화 도구를 활용한 개념 설명과 군더더기 없는 핵심 코드를 통해 개념과 구현...");
		two.setBookId("ISBN1235");
		two.setUnitInStock(100);
		two.setTotalPages(100);
		two.setReleaseDate("2018/03/02");
		two.setName("쉽게 배우는 자바 프로그래밍");
		two.setCategory("IT 모바일");
		two.setAuthor("우종증");
		two.setPublisher("한빛아카데미");
		two.setUnitPrice(27000);
		two.setCondition("Old");
		two.setFilename("ISBN1235.jpg");
		
		Book thr = new Book();
		thr.setDescription("스프링은 단순히 사용 방법만 익히는 것보다 아키텍처를 어떻게 이해하고 설계하는지가 더 중요합니다."
				+ "예제를 복사해 붙여넣는 식으로는 실제 개발에서 스프링을 제대로 활용할 수 없습니다...");
		thr.setBookId("ISBN1236");
		thr.setUnitInStock(50);
		thr.setTotalPages(498);
		thr.setReleaseDate("2018/03/02");
		thr.setName("스프링4 입문");
		thr.setCategory("IT 모바일");
		thr.setAuthor("하세가와 유이치, 오오노 와타루, 토키 코헤이(권은철, 전민수)");
		thr.setPublisher("한빛미디어");
		thr.setUnitPrice(27000);
		thr.setCondition("E-book");
		thr.setFilename("ISBN1236.jpg");
		
		listOfBooks.add(one);
		listOfBooks.add(two);
		listOfBooks.add(thr);
	}
	
	public ArrayList<Book> getAllBooks() {
		return listOfBooks;
	}
	
	public Book getBookById(String bookId) {
		Book bookById = null;
		
		for (int i = 0; i < listOfBooks.size(); i++) {
			Book book = listOfBooks.get(i);
			if (book != null && book.getBookId() != null && book.getBookId().equals(bookId)) {
				bookById = book;
				break;
			}
		}
		return bookById;
	}
	
	public void addBook (Book book) {
		listOfBooks.add(book);
	}
}
