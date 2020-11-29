package mvc.database;

import java.sql.Connection;
import java.sql.SQLException;
import java.sql.DriverManager;

public class DBConnection {
	
	public static Connection getConnection()throws SQLException, ClassNotFoundException  {		

		Connection conn = null;		
	
		String url = "jdbc:mysql://picdb.chbkhuyccdgh.ap-northeast-2.rds.amazonaws.com:3306/PicDB";
		String user = "admin";
		String password = "qwer1234";

		Class.forName("com.mysql.jdbc.Driver");
		conn = DriverManager.getConnection(url, user, password);		
		
		return conn;
	}	
}
