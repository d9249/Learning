#include <stdio.h>
#include <stdlib.h>

struct node {
	int data;
	struct node * prev;
	struct node * next;
}*head, *last;

void createList(int n);
void displayList();
void insertAtBeginning(int data);
void insertAtEnd(int data);
void insertAtN(int data, int position);

int main()
{
	int n, data, choice;

	head = NULL;
	last = NULL;

	printf("node number : ");
	scanf("%d", &choice);
	for (int i = 0; i < choice; i++) {
		printf("생성 노드 개수 : ");
		scanf("%d", &n);
		createList(n);
	}
	printf("plus node : ");
	scanf("%d", &data);
	insertAtBeginning(data);
	displayList();
}

void createList(int n)
{
	int i, data;
	struct node *newNode;

	if (n >= 1)
	{
		head = (struct node *)malloc(sizeof(struct node));

		printf("input 1 node : ");
		scanf("%d", &data);

		head->data = data;
		head->prev = NULL;
		head->next = NULL;

		last = head;

		for (i = 2; i <= n; i++)
		{
			newNode = (struct node *)malloc(sizeof(struct node));

			printf("input %d node : ", i);
			scanf("%d", &data);

			newNode->data = data;
			newNode->prev = last; // Link new node with the previous node
			newNode->next = NULL;

			last->next = newNode; // Link previous node with the new node
			last = newNode;          // Make new node as last/previous node
		}
	}
}

void displayList()
{
	struct node * temp;
	int n = 1;

	if (head == NULL)
	{
		printf("List is empty.\n");
	}
	else
	{
		temp = head;
		printf("DATA IN THE LIST:\n");

		while (temp != NULL)
		{
			printf("%d ", temp->data);

			n++;

			temp = temp->next;
		}
	}
}


/**
 * Inserts a new node at the beginning of the doubly linked list
 * @data Data of the first node i.e. data of the new node
 */
void insertAtBeginning(int data)
{
	struct node * newNode;

	if (head == NULL)
	{
		printf("Error, List is Empty!\n");
	}
	else
	{
		newNode = (struct node *)malloc(sizeof(struct node));

		newNode->data = data;
		newNode->next = head; // Point to next node which is currently head
		newNode->prev = NULL; // Previous node of first node is NULL

		/* Link previous address field of head with newnode */
		head->prev = newNode;

		/* Make the new node as head node */
		head = newNode;

	}
}


/**
 * Inserts a new node at the end of the doubly linked list
 * @data Data of the last node i.e data of the new node
 */
void insertAtEnd(int data)
{
	struct node * newNode;

	if (last == NULL)
	{
		printf("Error, List is empty!\n");
	}
	else
	{
		newNode = (struct node *)malloc(sizeof(struct node));

		newNode->data = data;
		newNode->next = NULL;
		newNode->prev = last;

		last->next = newNode;
		last = newNode;

	}
}

void insertAtN(int data, int position)
{
	int i;
	struct node * newNode, *temp;

	if (head == NULL)
	{
		printf("Error, List is empty!\n");
	}
	else
	{
		temp = head;
		i = 1;

		while (i < position - 1 && temp != NULL)
		{
			temp = temp->next;
			i++;
		}

		if (position == 1)
		{
			insertAtBeginning(data);
		}
		else if (temp == last)
		{
			insertAtEnd(data);
		}
		else if (temp != NULL)
		{
			newNode = (struct node *)malloc(sizeof(struct node));

			newNode->data = data;
			newNode->next = temp->next; // Connect new node with n+1th node
			newNode->prev = temp;          // Connect new node with n-1th node

			if (temp->next != NULL)
			{
				/* Connect n+1th node with new node */
				temp->next->prev = newNode;
			}
			/* Connect n-1th node with new node */
			temp->next = newNode;

			printf("NODE INSERTED SUCCESSFULLY AT %d POSITION\n", position);
		}
		else
		{
			printf("Error, Invalid position\n");
		}
	}
}