<%@ page language="java" contentType="text/html; charset=utf-8"
	pageEncoding="EUC-KR"%>
<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<title>페이지 오류</title>
<link rel="stylesheet"
	href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css">
</head>
<body>
	<%@ include file="menu.jsp"%>

	<div class="jumbotron">
		<div class="container">
			<h2 class="alret alert-danger">요청하신 페이지를 찾을 수 없습니다.</h2/>
		</div>
	</div>
	<div class="container">
		<%=request.getRequestURL()%>
		<br>
		<br> <a href="./books.jsp" class="btn btn-info">도서목록 &raquo;</a>
	</div>
</body>
</html>