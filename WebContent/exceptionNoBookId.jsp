<%@ page language="java" contentType="text/html; charset=utf-8"
	pageEncoding="EUC-KR"%>
<!DOCTYPE html>
<head>
<title>상품ID 오류</title>
<link rel="stylesheet"
	href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css">
</head>
<body>
	<%@ include file="menu.jsp"%>
	<div class="jumbotron">
		<div class="container">
			<h2 class="alret alert-danger">해당 페이지가 존재하지 않습니다.</h2/>
		</div>
	</div>
	<div class="container">
		<p><%=request.getRequestURL()%>?<%=request.getQueryString()%>
		<br>
		<br> <a href="./books.jsp" class="btn btn-info">Main page &raquo;</a>
	</div>
</body>
</html>