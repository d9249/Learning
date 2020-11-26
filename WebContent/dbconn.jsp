<%@ page import="java.sql.*" %>
<%
	Connection conn = null;
	
	try {
		String url = "jdbc:mysql://localhost:3306/BookMarketDB?autoReconnect=true&useSSL=false";
		String user = "root";
		String password = "1234";
		
		Class.forName("com.mysql.jdbc.Driver");
		
		conn = DriverManager.getConnection(url, user, password);
		}
	catch (SQLException ex) {
		out.println("No. <br>");
		out.println("SQLException: "+ex.getMessage());
	}
%>