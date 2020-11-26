<%@ page contentType="text/html; charset=UTF-8"%>
<%@ page import="com.oreilly.servlet.*" %>
<%@ page import="com.oreilly.servlet.multipart.*" %>
<%@ page import="java.util.*" %>
<%@ page import="java.sql.*" %>
<%@ include file="dbconn.jsp" %>
<%
	String filename = "";
	String realFolder = "/Users/mean/web/BookMarket/WebContent/resources/images/"; 	//웹 애플리케이션의 절대 경로
	String encType = "utf-8";
	int maxSize = 5 * 1024 * 1024;
	
	MultipartRequest multi = new MultipartRequest(request, realFolder, maxSize, encType, new DefaultFileRenamePolicy());
	String bookId = multi.getParameter("bookId");
	String name = multi.getParameter("name");
	String unitPrice = multi.getParameter("unitPrice");
	String author = multi.getParameter("author");
	String description = multi.getParameter("description");
	String publisher = multi.getParameter("publisher");
	String category = multi.getParameter("category");
	String unitInStock = multi.getParameter("unitInStock");
	String totalPages = multi.getParameter("totalPages");
	String releaseDate = multi.getParameter("ReleaseDate");
	String condition = multi.getParameter("condition");

	long stock;
	Integer price;
	long pages;

	
	if (unitPrice.isEmpty())
		price = 0;
	else
		price = Integer.valueOf(unitPrice);
	
	if (unitInStock.isEmpty())
		stock = 0;
	else
		stock = Long.valueOf(unitInStock);
	
	if (totalPages.isEmpty())
		pages = 0;
	else
		pages = Long.valueOf(totalPages);
	
	Enumeration files = multi.getFileNames();
	String fname = (String) files.nextElement();
	String fileName = multi.getFilesystemName(fname);
	
	PreparedStatement pstmt = null;
	ResultSet rs = null;
	
	String sql = "select * from book where b_id = ?";
	pstmt = conn.prepareStatement(sql);
	pstmt.setString(1, bookId);
	rs = pstmt.executeQuery();
	
	if (rs.next()) {
		if (fileName != null) {
			sql = "UPDATE BookMarketDB.book SET b_name=?, b_unitPrice=?, b_author=?, b_publisher=?, b_description=?, b_category=?, b_unitInStock=?, b_totalPages=?, b_releaseDate=?, b_condition=?, b_filename=? where b_id=?";
			pstmt = conn.prepareStatement(sql);
			pstmt.setString(1, name);
			pstmt.setInt(2, price);
			pstmt.setString(3, author);
			pstmt.setString(4, description);
			pstmt.setString(5, publisher);
			pstmt.setString(6, category);
			pstmt.setLong(7, stock);
			pstmt.setLong(8, pages);
			pstmt.setString(9, releaseDate);
			pstmt.setString(10, condition);
			pstmt.setString(11, filename);
			pstmt.setString(12, bookId);
			pstmt.executeUpdate();
		} else {
			sql = "UPDATE BookMarketDB.book SET b_name=?, b_unitPrice=?, b_author=?, b_publisher=?, b_description=?, b_category=?, b_unitInStock=?, b_totalPages=?, b_releaseDate=?, b_condition=? where b_id=?";
			pstmt = conn.prepareStatement(sql);
			pstmt.setString(1, name);
			pstmt.setInt(2, price);
			pstmt.setString(3, author);
			pstmt.setString(4, description);
			pstmt.setString(5, publisher);
			pstmt.setString(6, category);
			pstmt.setLong(7, stock);
			pstmt.setLong(8, pages);
			pstmt.setString(9, releaseDate);
			pstmt.setString(10, condition);
			pstmt.setString(11, bookId);
			pstmt.executeUpdate();
		}
	}
	if (rs != null)
	rs.close();
	if (pstmt != null)
		pstmt.close();
	if (conn != null)
		conn.close();
	response.sendRedirect("editBook.jsp?edit=update");
%>