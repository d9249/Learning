<%@ page contentType="text/html; charset=UTF-8"%>
<%@ page import="java.sql.*" %>
<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<link rel="stylesheet" href="./resources/css/bootstrap.min.css">
<title>상품 편집</title>
<script type="text/javascript">
	function deleteConfirm(id) {
		if (confirm("해당 상품을 삭제합니다!!") == true)
			location.href = "./deleteBook.jsp?id=" + id;
		else
			return;
	}
</script>
</head>
<%
	String edit = request.getParameter("edit");
%>
<body>
	<%@include file="menu.jsp" %>
	<div class="jumbotron">
		<div class="container">
			<h1 class="display-3">상품 편집</h1>
		</div>
	</div>
	<!--  -->
	<div class="container">
		<div class="col" align="left">
			<%@ include file="dbconn.jsp" %>
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
 				<img src="./resources/images/<%=rs.getString("b_filename") %>" style="width:200px; margin-right:40px">
 			</div>
			<div class="">
				<h3><b>[<%=rs.getString("b_category")%>] <%=rs.getString("b_name")%></b></h3>
				<br>
				<p><%=rs.getString("b_description") %></p>		
				<p><%=rs.getString("b_author") %> | <%=rs.getString("b_publisher") %> | <%=rs.getString("b_unitPrice") %> won</p>
 				</div>
			<div class="">
				<%
					if (edit.equals("update")) {
				%>
				<a href="./updateBook.jsp?id=<%=rs.getString("b_id")%>" class="btn btn-primary" role="button" style="width:120px; margin-left:15px; padding=15px;"> 수정 &raquo;</a>
				<%
					} else if (edit.equals("delete")) {
				%>
				<a href="#" onclick="deleteConfirm('<%=rs.getString("b_id")%>')" class="btn btn-danger" role="button" style="width:120px; margin-left:15px; padding=15px;"> 삭제 &raquo;</a>
			</div>
				<%
					}
				%>
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
	<%@ include file="footer.jsp"%>
</body>
</html>