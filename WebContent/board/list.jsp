<%@ page contentType="text/html; charset=utf-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core"%>
<%@ page import="java.util.*"%>
<%@ page import="mvc.model.BoardDTO"%>

<%
	String sessionId = (String) session.getAttribute("sessionId");
	List boardList = (List) request.getAttribute("boardlist");
	int total_record = ((Integer) request.getAttribute("total_record")).intValue();
	int pageNum = ((Integer) request.getAttribute("pageNum")).intValue();
	int total_page = ((Integer) request.getAttribute("total_page")).intValue();
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
<%@ taglib prefix="fmt" uri="http://java.sun.com/jsp/jstl/fmt"%>
<head>
<link rel="stylesheet" href="./resources/css/bootstrap.min.css" />
<meta name="viewport" content="width=device-width,initial-scale=1">
<title>Board</title>
<script type="text/javascript">
	function checkForm() {	
		if (${sessionId==null}) {
			alert("로그인 해주세요.");
			return false;
		}
		location.href = "./BoardWriteForm.do?id=<%=sessionId%>"
	}
</script>
<style>
	#nill {
		display: grid; grid-template-rows: repeat(2, 450px); grid-template-columns: repeat(2, 1fr); gap: 20px 10px;
	}
	
	@media screen and (max-width: 1000px) { 
		#nill { 
			display: grid; 
			grid-template-rows: repeat(4, 450px); 
			grid-template-columns: repeat(1, 1fr); 
			gap: 20px 10px; 
		} 
	}
</style>
</head>
<body>
	<fmt:setLocale value='<%=request.getParameter("language")%>' />
	<fmt:setBundle basename="bundle.Translation"/>
	<fmt:bundle basename="bundle.message" />
	<header>
	<div class="container" style="padding-top: 8px; padding-bottom: 8px;">
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
	</header>
	<br>
	<br>
	<div class="jumbotron" style="background-color: #FFFFFF">
		<div class="container">
			<h1 class="display-3" align="right">
				<p style="font-family: 'Nanum Myeongjo', sans-serif;">
					<b><fmt:message key="text11"/></b>
				</p>
			</h1>
			<h2 class="display-4" align="right">
				<p style="font-family: 'Nanum Myeongjo', sans-serif;"><fmt:message key="text12"/></p>
			</h2>
		</div>
	</div>
	<div class="container">
		<form action="<c:url value="./BoardListAction.do"/>" method="post">
			<div>
				<div class="text-right">
					<div>
						<a href="?language=ko">Korean</a> | <a href="?language=en">English</a>
					</div>
					<br>
					<span class="badge badge-success"><fmt:message key="text13"/> : <%=total_record%>
					</span>
				</div>
			</div>
			<div id="ni1" style="padding-top: 50px">
				<div class="container" id="nill">
					<%
						for (int j = 0; j < boardList.size(); j++) {
						BoardDTO notice = (BoardDTO) boardList.get(j);
					%>
					<div align="center">
						<a href="./BoardViewAction.do?num=<%=notice.getNum()%>&pageNum=<%=pageNum%>">
						<img src="./resources/images/<%=notice.getFilename() %>" style="height: 300px; margin-right: 40px"></a>
						<h8> </h8>
						<h5>
							<b><fmt:message key="text14"/> : <%=notice.getSubject()%></b>
						</h5>
						<h5>
							<b><fmt:message key="text15"/> : <%=notice.getDescription()%></b>
						</h5>
						<h5>
							<b><fmt:message key="text16"/> : <%=notice.getName()%></b>
						</h5>
						<h5>
							<b><fmt:message key="text17"/> : <%=notice.getHit()%></b>
						</h5>
					</div>
					<%
						}
					%>
				</div>
			</div>
			<div align="center">
				<c:set var="pageNum" value="<%=pageNum%>" />
				<c:forEach var="i" begin="1" end="<%=total_page%>">
					<a href="<c:url value="./BoardListAction.do?pageNum=${i}" /> ">
						<c:choose>
							<c:when test="${pageNum==i}">
								<font color='4C5317'><b> [${i}]</b></font>
							</c:when>
							<c:otherwise>
								<font color='4C5317'> [${i}]</font>
							</c:otherwise>
						</c:choose>
					</a>
				</c:forEach>
			</div>
			<div align="left">
				<table>
					<tr>
						<td width="100%" align="left" style="width:100%; height:100px;">&nbsp;&nbsp; 
							<select	name="items" class="txt" style="height: 40px;">
								<option value="subject"><fmt:message key="text18"/></option>
								<option value="description"><fmt:message key="text19"/></option>
								<option value="name"><fmt:message key="text20"/></option>
							</select>
							<input name="text" type="text" style="height: 40px;"/> 
							<input type="submit" id="btnAdd" class="btn btn-primary" style="height: 43.5px;" value="검색"/>
						</td>
						<td width="100%" align="right">
							<a href="#"	onclick="checkForm(); return false;" class="btn btn-primary"
								role="button" style="width: 140px; height:43.5px; margin-left: 10px; padding-top:8px;">&laquo; <fmt:message key="text4"/>  </a>
						</td>
					</tr>
				</table>
			</div>
		</form>
		<hr>
	</div>
	<jsp:include page="../footer.jsp" />
</body>
</html>





