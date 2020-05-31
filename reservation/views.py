from django.shortcuts import render, redirect,get_object_or_404
from .models import reservation,RImg_board
from .forms import reservation, RImg_Board
from django.utils import timezone
from django.core.paginator import Paginator
from django.http import HttpResponse
# Create your views here.

def readreservation(request):
    Rimg_boards = RImg_board.objects
    Rimg_board_list = RImg_board.objects.all()
    paginator = Paginator(Rimg_board_list,3)
    page = request.GET.get('page')
    posts = paginator.get_page(page)
    return render(request,'readreservation.html',{'Rimg_boards':Rimg_boards,'posts':posts})

def searchreservation(request):
    query = request.GET['query']
    img = RImg_board.objects.all()
    if query :
        Rimg_boards = img.filter(title__contains=query)
        return render(request, 'reservationsearch.html',{'Rimg_boards':Rimg_boards})

def detailreservation(request,Rimg_board_id):
    Rimg_board_detail = get_object_or_404(RImg_board,pk=Rimg_board_id)
    return render(request,'detailreservation.html',{'Rimg_board_detail':Rimg_board_detail})    

def postreservation(request):
    if request.method =="POST":
        form = RImg_Board(request.POST, request.FILES)
        if form.is_valid():
            form.save()
            return redirect('readreservation')
    else:
        form = RImg_Board()
        return render(request, 'reservation.html',{'form':form})
        #수정하기 눌렀을때 아무것도 입력안받아서 else 실행되서 createboard.html뜨는건 알겠는데 왜거기서 작성하면 또 post로 올까?    

def updatereservation(request,pk):
    Rimg_board = get_object_or_404(RImg_board, pk=pk)
    
    form = RImg_Board(request.POST,request.FILES, instance=Rimg_board)

    if form.is_valid():
        form.save()
        return redirect('readreservation')
    else:        
        return render(request, 'reservation.html',{'form':form})    

def deletereservation(request, pk):
    Rimg_board = get_object_or_404(RImg_board, pk=pk)
    Rimg_board.delete()
    return redirect('readreservation')

