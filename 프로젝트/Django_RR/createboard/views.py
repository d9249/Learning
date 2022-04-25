from django.shortcuts import render, redirect,get_object_or_404
from .models import Board,Img_board
from .forms import CreateBoard, Img_Board
from django.utils import timezone
from django.core.paginator import Paginator
from django.http import HttpResponse
# Create your views here.

def readboard(request):
    img_boards = Img_board.objects
    img_board_list = Img_board.objects.all()
    paginator = Paginator(img_board_list,3)
    page = request.GET.get('page')
    posts = paginator.get_page(page)
    return render(request,'readboard.html',{'img_boards':img_boards,'posts':posts})


def search(request):
    query = request.GET['query']
    img = Img_board.objects.all()
    if query :
        img_boards = img.filter(title__contains=query)
        return render(request, 'search.html',{'img_boards':img_boards})



def detail(request,img_board_id):
    img_board_detail = get_object_or_404(Img_board,pk=img_board_id)
    return render(request,'detail.html',{'img_board_detail':img_board_detail})    


def post(request):
    if request.method =="POST":
        form = Img_Board(request.POST, request.FILES)
        if form.is_valid():
            form.save()
            return redirect('readboard')
            
    else:
        form = Img_Board()
        return render(request, 'createboard.html',{'form':form})
        #수정하기 눌렀을때 아무것도 입력안받아서 else 실행되서 createboard.html뜨는건 알겠는데 왜거기서 작성하면 또 post로 올까?    

def update(request,pk):
    img_board = get_object_or_404(Img_board, pk=pk)
    
    form = Img_Board(request.POST,request.FILES, instance=img_board)

    if form.is_valid():
        form.save()
        return redirect('readboard')
    else:        
        return render(request, 'createboard.html',{'form':form})    



def delete(request, pk):
    img_board = get_object_or_404(Img_board, pk=pk)
    img_board.delete()
    return redirect('readboard')

