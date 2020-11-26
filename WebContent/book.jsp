<%@ page language="java" contentType="text/html; charset=EUC-KR"
    pageEncoding="EUC-KR"%>
<%@ page import="dto.Book" %>
<%@ page import="dao.BookRepository" %>
<%@ page errorPage="exceptionNoBookId.jsp" %>
<!DOCTYPE html>
<html>
<head>
<link rel="stylesheet" href="./resources/css/bootstrap.min.css">
<meta charset="EUC-KR">
<title>상세 정보</title>
<script type="text/javascript">
	function addToCart() {
		if (confirm("상품을 장바구니에 추가하시겠습니까?")){
			document.addForm.submit();
		} else {
			document.addForm.reset();
		}
	}
</script>
</head>
<body>
	<%@ include file="menu.jsp" %>
	<%! String greeting = "도서 정보"; %>
	<div class = "jumbotron">
		<div class = "container">
			<h1 class = "display-3">
				<%= greeting %>
			</h1/>
		</div>
	</div>
<%-- 	<% 
		String id = request.getParameter("id");
		BookRepository dao = BookRepository.getInstance();
		Book book = dao.getBookById(id);
	%> --%>
	<%@ include file="dbconn.jsp" %>
	<%	
		String bookId = request.getParameter("id");

		PreparedStatement pstmt = null;
		ResultSet rs = null;
		String sql = "select * from book where b_id =?";
		pstmt = conn.prepareStatement(sql);
		pstmt.setString(1, bookId);
		rs =  pstmt.executeQuery();
		while (rs.next()) {
	%>
	<div class="container">
		<div class="row">
			<div class="col-md-5 col-sm-4">
				<img src="./resources/images/<%=rs.getString("b_filename") %>" style="width: 100%">
			</div>
			<div class="cod-md-6 col-sm-6">
				<h3><b>[<%=rs.getString("b_category")%>] <%=rs.getString("b_name")%></b></h3>
				<br>
				<p><%=rs.getString("b_description") %></p>
				<p><b>도서코드 : </b><span class="badge badge-secondary"><%=rs.getString("b_id")%></p></span>
				<p><b>출판사 : </b><%=rs.getString("b_publisher") %></p>
				<p><b>저자 : </b><%=rs.getString("b_author")%></p>
				<p><b>재고수 : </b><%=rs.getString("b_unitInStock")%></p>
				<p><b>총 페이지수 : </b><%=rs.getString("b_totalPages")%></p>
				<p><b>출판일 : </b><%=rs.getString("b_releaseDate")%></p>
				<h2><p><%=rs.getString("b_unitPrice")%>원</p></h2>
				<p><form name="addForm" action="./addCart.jsp?id=<%=rs.getString("b_id")%>" method="post">
					<a href="#" class="btn btn-info" onclick="addToCart()">도서 주문 &raquo;</a>
					<a href="./cart.jsp" class="btn btn-warning">장바구니 &raquo;</a>
					<a href="./books.jsp" class="btn btn-info">도서 목록 &raquo;</a>
				</form>
				<hr>
			</div>
		</div>
	</div>
	<%
		}
		if (rs != null)
			rs.close();
		if (pstmt != null)
			pstmt.close();
		if (conn != null)
			conn.close();
	%>
	<%@ include file="footer.jsp"%>
</body>
</html>