<%@ page contentType="text/html; charset=euc-kr"%>
<%@ taglib prefix="fmt" uri="http://java.sun.com/jsp/jstl/fmt"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core"%>
<%
	request.setCharacterEncoding("euc-kr");
%>
<%@ page import="java.util.ArrayList"%>
<%@ page import="java.sql.*"%>
<html>
<head>
<title>Pic</title>
<meta charset=utf-8>
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

</head>
<body>
	<header>
	<div class="container">
		<div class="colorlib-navbar-brand">
			<a class="colorlib-logo" style="color:black;" href="./books.jsp">Pic | </a>
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
	<br>
	<br>
	<fmt:setLocale value='<%=request.getParameter("language")%>' />
	<fmt:setBundle basename="bundle.Translation"/>
	<fmt:bundle basename="bundle.message" />
	</header>
		<section class="hero-wrap js-fullheight">
			<div class="container-fluid px-0">
				<div
					class="row no-gutters slider-text js-fullheight align-items-center justify-content-center"
					data-scrollax-parent="true">
					<div class="col-md-12 ftco-animate text-center">
						<div class="desc">
							<span class="subheading" style="">memory</span>
							<h1 style="background-image: url(./resources/images/bg_1.jpg);">Pic</h1>
							<span class="subheading-2">sharing</span>
						</div>
					</div>
				</div>
			</div>
		</section>

		<section class="ftco-section ftco-no-pb ftco-no-pt">
			<div class="container-fluid px-0">
				<div class="row no-gutters">
					<div class="col-md-12 blog-wrap">
						<div class="row no-gutters align-items-center">
							<div class="col-md-6 img js-fullheight"
								style="background-image: url(./resources/images/image_1.jpg);"></div>
							<div class="col-md-6">
								<div class="text p-md-5 p-4 ftco-animate">
									<a href="?language=ko">Korean</a> | <a href="?language=en">English</a>
									<h2 class="mb-4">
										<a href="blog-single.html"><fmt:message key="text1"/></a>
									</h2>
									<p><fmt:message key="text2"/></p>
									<p><fmt:message key="text3"/></p>
									<p class="mb-0 mt-4">
										<a href="<c:url value="/BoardListAction.do?pageNum=1"/>" class="btn btn-primary"><fmt:message key="text4"/></a>
									</p>
								</div>
							</div>
						</div>
					</div>
					<div class="col-md-12 blog-wrap">
						<div class="row no-gutters align-items-center">
							<div class="col-md-6 img js-fullheight order-md-last"
								style="background-image: url(./resources/images/image_2.jpg);"></div>
							<div class="col-md-6">
								<div class="text p-md-5 p-4 ftco-animate">
									<a href="?language=ko">Korean</a> | <a href="?language=en">English</a>
									<h2 class="mb-4">
										<a href="blog-single.html"><fmt:message key="text5"/></a>
									</h2>
									<p><fmt:message key="text6"/></p>
									<p class="mb-0 mt-4">
										<a href="<c:url value="/BoardListAction.do?pageNum=1"/>" class="btn btn-primary"><fmt:message key="text4"/></a>
									</p>
								</div>
							</div>
						</div>
					</div>
					<div class="col-md-12 blog-wrap">
						<div class="row no-gutters align-items-center">
							<div class="col-md-6 img js-fullheight"
								style="background-image: url(./resources/images/image_8.jpg);"></div>
							<div class="col-md-6">
								<div class="text p-md-5 p-4 ftco-animate">
									<a href="?language=ko">Korean</a> | <a href="?language=en">English</a>
									<h2 class="mb-4">
										<a href="blog-single.html"><fmt:message key="text7"/></a>
									</h2>
									<p><fmt:message key="text6"/></p>
									<p class="mb-0 mt-4">
										<a href="<c:url value="/BoardListAction.do?pageNum=1"/>" class="btn btn-primary"><fmt:message key="text4"/></a>
									</p>
								</div>
							</div>
						</div>
					</div>
					<div class="col-md-12 blog-wrap bg-darken">
						<div class="row no-gutters align-items-center">
							<div
								class="col-md-6 d-flex justify-content-center align-items-center order-md-last js-fullheight">
								<div class="img"
									style="background-image: url(./resources/images/image_4.jpg);"></div>
							</div>
							<div class="col-md-6">
								<div class="text p-md-5 p-4 ftco-animate">
									<a href="?language=ko">Korean</a> | <a href="?language=en">English</a>
									<h2 class="mb-4">
										<a href="blog-single.html"><fmt:message key="text8"/></a>
									</h2>
									<p><fmt:message key="text9"/></p>
									<p><fmt:message key="text10"/></p>
									<p class="mb-0 mt-4">
										<a href="<c:url value="/BoardListAction.do?pageNum=1"/>" class="btn btn-primary"><fmt:message key="text4"/></a>
									</p>
								</div>
							</div>
						</div>
					</div>
		</section>

		<footer class="ftco-footer ftco-section img">
			<div class="overlay"></div>
			<div class="container">
				<div class="row">
					<div class="col-md-12 text-center">
						<p>
							<!-- Link back to Colorlib can't be removed. Template is licensed under CC BY 3.0. -->
							Copyright
							<script>
								document.write(new Date().getFullYear());
							</script>
							All rights reserved | This template is made with by <a
								href="https://colorlib.com" target="_blank">Colorlib</a> <br>
							Copyright
							<script>
								document.write(new Date().getFullYear());
							</script>
							All rights reserved | This site is made by mean_ideal</a>
							<!-- Link back to Colorlib can't be removed. Template is licensed under CC BY 3.0. -->
						</p>
					</div>
				</div>
			</div>
		</footer>
		<!-- loader -->
		<div id="ftco-loader" class="show fullscreen">
			<svg class="circular" width="48px" height="48px">
				<circle class="path-bg" cx="24" cy="24" r="22" fill="none"
					stroke-width="4" stroke="#eeeeee" />
				<circle class="path" cx="24" cy="24" r="22" fill="none"
					stroke-width="4" stroke-miterlimit="10" stroke="#F96D00" /></svg>
		</div>
	</div>

	<script src="./resources/js/jquery.min.js"></script>
	<script src="./resources/js/jquery-migrate-3.0.1.min.js"></script>
	<script src="./resources/js/popper.min.js"></script>
	<script src="./resources/js/bootstrap.min.js"></script>
	<script src="./resources/js/jquery.easing.1.3.js"></script>
	<script src="./resources/js/jquery.waypoints.min.js"></script>
	<script src="./resources/js/jquery.stellar.min.js"></script>
	<script src="./resources/js/owl.carousel.min.js"></script>
	<script src="./resources/js/jquery.magnific-popup.min.js"></script>
	<script src="./resources/js/aos.js"></script>
	<script src="./resources/js/jquery.animateNumber.min.js"></script>
	<script src="./resources/js/scrollax.min.js"></script>
	<script src="./resources/js/google-map.js"></script>
	<script src="./resources/js/main.js"></script>
</body>
</html>