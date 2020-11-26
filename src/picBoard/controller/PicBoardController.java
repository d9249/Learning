package picBoard.controller;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import javax.servlet.RequestDispatcher;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import picBoard.model.picBoardDAO;
import picBoard.model.picBoardDTO;

public class PicBoardController extends HttpServlet {
	private static final long serialVersionUID = 1L;
	static final int LISTCOUNT = 10; 

	public void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		doPost(request, response);
	}

	public void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		
		String RequestURI = request.getRequestURI();
		String contextPath = request.getContextPath();
		String command = RequestURI.substring(contextPath.length());
		
		response.setContentType("text/html; charset=utf-8");
		request.setCharacterEncoding("utf-8");
	
		if (command.equals("/PicBoardListAction.do")) {
			requestBoardList(request);
			RequestDispatcher rd = request.getRequestDispatcher("./picBoard/list.jsp");
			rd.forward(request, response);
		} else if (command.equals("/BoardWriteForm.do")) {
				requestLoginName(request);
				RequestDispatcher rd = request.getRequestDispatcher("./PicBoard/writeForm.jsp");
				rd.forward(request, response);				
		} else if (command.equals("/BoardWriteAction.do")) {
				requestBoardWrite(request);
				RequestDispatcher rd = request.getRequestDispatcher("/PicBoardListAction.do");
				rd.forward(request, response);						
		} else if (command.equals("/BoardViewAction.do")) {
				requestBoardView(request);
				RequestDispatcher rd = request.getRequestDispatcher("/PicBoardView.do");
				rd.forward(request, response);						
		} else if (command.equals("/BoardView.do")) { 
				RequestDispatcher rd = request.getRequestDispatcher("./picBoard/view.jsp");
				rd.forward(request, response);	
		} else if (command.equals("/BoardUpdateAction.do")) { 
				requestBoardUpdate(request);
				RequestDispatcher rd = request.getRequestDispatcher("/PicBoardListAction.do");
				rd.forward(request, response);
		}else if (command.equals("/BoardDeleteAction.do")) { 
				requestBoardDelete(request);
				RequestDispatcher rd = request.getRequestDispatcher("/PicBoardListAction.do");
				rd.forward(request, response);				
		}
	}
	
	public void requestBoardList(HttpServletRequest request){
			
		picBoardDAO dao = picBoardDAO.getInstance();
		List<picBoardDTO> picboardlist = new ArrayList<picBoardDTO>();
		
	  	int pageNum=1;
		int limit=LISTCOUNT;
		
		if(request.getParameter("pageNum")!=null)
			pageNum=Integer.parseInt(request.getParameter("pageNum"));
				
		String items = request.getParameter("items");
		String text = request.getParameter("text");
		
		int total_record=dao.getListCount(items, text);
		picboardlist = dao.getBoardList(pageNum,limit, items, text); 
		
		int total_page;
		
		if (total_record % limit == 0){     
	     	total_page =total_record/limit;
	     	Math.floor(total_page);  
		}
		else{
		   total_page =total_record/limit;
		   Math.floor(total_page); 
		   total_page =  total_page + 1; 
		}		
   
   		request.setAttribute("pageNum", pageNum);		  
   		request.setAttribute("total_page", total_page);   
		request.setAttribute("total_record",total_record); 
		request.setAttribute("boardlist", picboardlist);		
	}
	
	public void requestLoginName(HttpServletRequest request){
					
		String id = request.getParameter("id");
		
		picBoardDAO  dao = picBoardDAO.getInstance();
		
		String name = dao.getLoginNameById(id);		
		
		request.setAttribute("name", name);									
	}
	
	public void requestBoardWrite(HttpServletRequest request){
					
		picBoardDAO dao = picBoardDAO.getInstance();		
		
		picBoardDTO picboard = new picBoardDTO();
		picboard.setId(request.getParameter("id"));
		picboard.setName(request.getParameter("name"));
		picboard.setSubject(request.getParameter("subject"));
		picboard.setAddress(request.getParameter("address"));
		picboard.setDescription(request.getParameter("description"));
		picboard.setCamera(request.getParameter("camera"));
		picboard.setPhotoTime(request.getParameter("photoTime"));
		picboard.setCategory(request.getParameter("category"));
		
		System.out.println(request.getParameter("name"));
		System.out.println(request.getParameter("subject"));
		System.out.println(request.getParameter("address"));
		System.out.println(request.getParameter("description"));
		System.out.println(request.getParameter("camera"));
		System.out.println(request.getParameter("photoTime"));
		System.out.println(request.getParameter("category"));
		java.text.SimpleDateFormat formatter = new java.text.SimpleDateFormat("yyyy/MM/dd(HH:mm:ss)");
		String regist_day = formatter.format(new java.util.Date()); 
		
		picboard.setHit(0);
		picboard.setRegist_day(regist_day);
		picboard.setIp(request.getRemoteAddr());			
		
		dao.insertBoard(picboard);								
	}
	
	public void requestBoardView(HttpServletRequest request){
					
		picBoardDAO dao = picBoardDAO.getInstance();
		int num = Integer.parseInt(request.getParameter("num"));
		int pageNum = Integer.parseInt(request.getParameter("pageNum"));	
		
		picBoardDTO picboard = new picBoardDTO();
		picboard = dao.getBoardByNum(num, pageNum);		
		
		request.setAttribute("num", num);		 
   		request.setAttribute("page", pageNum); 
   		request.setAttribute("board", picboard);   									
	}
	
	public void requestBoardUpdate(HttpServletRequest request){
					
		int num = Integer.parseInt(request.getParameter("num"));
		int pageNum = Integer.parseInt(request.getParameter("pageNum"));	
		
		picBoardDAO dao = picBoardDAO.getInstance();		
		
		picBoardDTO picboard = new picBoardDTO();		
		picboard.setNum(num);
		picboard.setName(request.getParameter("name"));
		picboard.setSubject(request.getParameter("subject"));
		picboard.setAddress(request.getParameter("address"));
		picboard.setDescription(request.getParameter("description"));
		picboard.setCamera(request.getParameter("camera"));
		picboard.setPhotoTime(request.getParameter("photoTime"));
		picboard.setCategory(request.getParameter("category"));
		
		 java.text.SimpleDateFormat formatter = new java.text.SimpleDateFormat("yyyy/MM/dd(HH:mm:ss)");
		 String regist_day = formatter.format(new java.util.Date()); 
		 
		 picboard.setHit(0);
		 picboard.setRegist_day(regist_day);
		 picboard.setIp(request.getRemoteAddr());			
		
		 dao.updateBoard(picboard);								
	}
	
	public void requestBoardDelete(HttpServletRequest request){
					
		int num = Integer.parseInt(request.getParameter("num"));
		int pageNum = Integer.parseInt(request.getParameter("pageNum"));	
		
		picBoardDAO dao = picBoardDAO.getInstance();
		dao.deleteBoard(num);							
	}	
}
