<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core"%>
<%
	String sessionId = (String) session.getAttribute("sessionId");
%>
<link rel="stylesheet" href="./resources/css/bootstrap.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
<link href="https://fonts.googleapis.com/css?family=Poppins:100,200,300,400,500,600,700,800,900" rel="stylesheet">
<link href="https://fonts.googleapis.com/css?family=Abril+Fatface&display=swap" rel="stylesheet">
<link rel="stylesheet" href="./resources/css/open-iconic-bootstrap.min.css">
<link rel="stylesheet" href="./resources/css/animate.css">
<link rel="stylesheet" href="./resources/css/owl.carousel.min.css">
<link rel="stylesheet" href="./resources/css/owl.theme.default.min.css">
<link rel="stylesheet" href="./resources/css/magnific-popup.css">
<link rel="stylesheet" href="./resources/css/aos.css">
<link rel="stylesheet" href="./resources/css/ionicons.min.css">
<link rel="stylesheet" href="./resources/css/bootstrap-datepicker.css">
<link rel="stylesheet" href="./resources/css/jquery.timepicker.css">
<link rel="stylesheet" href="./resources/css/flaticon.css">
<link rel="stylesheet" href="./resources/css/icomoon.css">
<link rel="stylesheet" href="./resources/css/style.css">

<header>
	<div class="container">
		<div class="colorlib-navbar-brand">
			<a class="colorlib-logo" style="color:black;" href="../books.jsp">Pic | </a>
			<c:choose>
				<c:when test="${empty sessionId}">
					<a class="colorlib-logo" style="color:black;" href="<c:url value="/member/loginMember.jsp"/>">Login | </a> 
					<a class="colorlib-logo" style="color:black;" href="<c:url value="/member/addMember.jsp"/>">Sign up | </a> 
				</c:when>
				<c:otherwise>
 					<a class="colorlib-logo" style="color:black;" href="<c:url value="/member/logoutMember.jsp"/>">Logout | </a> 
					<a class="colorlib-logo" style="color:black;" href="<c:url value="/member/updateMember.jsp"/>">My page | </a> 
				</c:otherwise>
			</c:choose>
			<a class="colorlib-logo" style="color:black;" href="<c:url value="/BoardListAction.do?pageNum=1"/>">Pic Board</a>
		</div>
	</div>
</header>
<br>
<br>