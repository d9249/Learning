<%@ page language="java" contentType="text/html; charset=utf-8" 
	pageEncoding="EUC-KR"%>
<html>
<head>
<title>Welcome</title>
<link rel = "stylesheet"
	href = "https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css">
</head>
<body>
	<%@ include file="menu.jsp" %>
	
	<%! String greeting = "Admin page"; %>
	
	<div class = "jumbotron">
		<div class = "container">
			<h1 class = "display-3">
				<%= greeting %>
			</h1/>
		</div>
	</div>
	<main role = "main">
	<div class = "text-center">
		<h3>
		</h3>
	</div>
	</main>
	<hr>
	<%@ include file="footer.jsp"%>
</body>
</html>