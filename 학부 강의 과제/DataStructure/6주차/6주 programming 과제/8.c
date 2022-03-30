#include<stdio.h>
#include<stdlib.h>

struct dllNode
{
	int data;
	struct dllNode *previous, *next;
};

struct dllNode *head = NULL;
static int totNodes = 0;

struct dllNode *createNode(int);
void insertNode(int);
void deleteNode(int);
void insertionSort();
void traverseList();

struct dllNode* createNode(int data)
{
	struct dllNode *nPtr = (struct dllNode *)malloc(sizeof(struct dllNode));
	nPtr->data = data;
	nPtr->previous = NULL;
	nPtr->next = NULL;
	return (nPtr);
}

void insertNode(int data)
{
	struct dllNode *tmp, *nPtr = createNode(data);
	if (!head)
	{
		head = nPtr;
		totNodes++;
		return;
	}
	else
	{
		tmp = head;
		while (tmp)
		{
			if (tmp->next == NULL)
			{
				tmp->next = nPtr;
				nPtr->previous = tmp;
				totNodes++;
				return;
			}
			tmp = tmp->next;
		}
	}
}

void deleteNode(int data)
{
	struct dllNode *nPtr, *tmp = head;
	if (head == NULL)
	{
		printf("Data unavailable \n");
		return;
	}
	else if (tmp->data == data)
	{
		nPtr = tmp->next;
		tmp->next = NULL;
		free(tmp);
		head = nPtr;
		totNodes--;
	}
	else
	{
		while (tmp->next != NULL && tmp->data != data)
		{
			nPtr = tmp;
			tmp = tmp->next;
		}
		if (tmp->next == NULL && tmp->data != data)
		{
			printf("Given data unavailable in list \n");
			return;
		}
		else if (tmp->next != NULL && tmp->data == data)
		{
			nPtr->next = tmp->next;
			tmp->next->previous = tmp->previous;
			tmp->next = NULL;
			tmp->previous = NULL;
			free(tmp);
			printf("Data deleted successfully \n");
			totNodes--;
		}
		else if (tmp->next == NULL && tmp->data == data)
		{
			nPtr->next = NULL;
			tmp->next = tmp->previous = NULL;
			free(tmp);
			printf("Data deleted successfully \n");
			totNodes--;
		}
	}
}

void insertionSort()
{
	struct dllNode *nPtr1, *nPtr2;
	int i, j, tmp;
	nPtr1 = nPtr2 = head;

	for (i = 0; i < totNodes; i++)
	{
		tmp = nPtr1->data;
		for (j = 0; j < i; j++)
			nPtr2 = nPtr2->next;
		for (j = i; j > 0 && nPtr2->previous->data < tmp; j--)
		{
			nPtr2->data = nPtr2->previous->data;
			nPtr2 = nPtr2->previous;
		}
		nPtr2->data = tmp;
		nPtr2 = head;
		nPtr1 = nPtr1->next;
	}
}

void traverseList()
{
	struct dllNode *nPtr = head;
	int i = 0;
	while (nPtr)
	{
		printf("%d ", nPtr->data);
		nPtr = nPtr->next;
		i++;
	}
}

int main()
{
	int ch, data, num;
	printf("element number : ");
	scanf("%d", &num);
	for (int i = 0; i < num; i++) {
		printf("input : ");
		scanf("%d", &data);
		insertNode(data);
	}
	printf("원본 리스트 : ");
	traverseList();
	printf("\n");
	printf("분류된 리스트 : ");
	insertionSort();
	traverseList();

	

	
}
