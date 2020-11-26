<%@ page language="java" contentType="text/html; charset=utf-8"%>
<%
	request.setCharacterEncoding("utf-8");
%>
<%@ page import="java.util.ArrayList" %>
<%@ page import="dto.Book" %>
<%@ page import="dao.BookRepository" %>
<%@ page import="java.sql.*" %>
<html>
<head>
<title>Book List</title>
<meta charset=utf-8>
<link rel="stylesheet" href="../resources/css/bootstrap.min.css">
</head>
<body>
	<%@ include file="../menu.jsp" %>
	<div class="jumbotron" style="background-color: #FFFFFF">
		<div class="container">
			<h1 class="display-3" align="right">
				<p style="font-family: 'Nanum Myeongjo', sans-serif;">
					<b>사진, 당신 인생 속 한 조각을</b>
				</p>
			</h1>
			<h2 class="display-4" align="right">
				<p style="font-family: 'Nanum Myeongjo', sans-serif;">공유 해주세요.</p>
			</h2>
		</div>
	</div>
	<div class="container">
		<div class="col" align="left">
			<%@ include file="../dbconn.jsp" %>
			<%
				PreparedStatement pstmt = null;
				ResultSet rs = null;
				String sql = "select * from book";
				pstmt = conn.prepareStatement(sql);
				rs =  pstmt.executeQuery();
				while (rs.next()) {
			%>
		<div class="container" style="display:flex;">	
			<div class="">
 				<img src="../resources/images/<%=rs.getString("b_filename") %>" style="width:200px; margin-right:40px">
 			</div>
			<div class="">
				<h3><b>[<%=rs.getString("b_category")%>] <%=rs.getString("b_name")%></b></h3>
				<br>
				<p><%=rs.getString("b_description") %></p>		
				<p><%=rs.getString("b_author") %> | <%=rs.getString("b_publisher") %> | <%=rs.getString("b_unitPrice") %> won</p>
 				</div>
			<div class="">
				<a href="./book.jsp?id=<%=rs.getString("b_id")%>" class="btn btn-secondary" role="button" style="width:120px; margin-left:15px; padding=15px;">detail &raquo;</a>
 			</div>
		</div>
		<hr>
			<%
						}
				if (rs != null)
					rs.close();
				if (pstmt != null)
					pstmt.close();
				if (conn != null)
					conn.close();
			%>
		</div>
	</div>
	<%@ include file="../footer.jsp"%>
</body>
</html>