#include <stdio.h>
#include <stdlib.h>

struct node {
	int data;    
	struct node *next;
}*head;

void createList(int n);
void deleteLastNode();
void displayList();
void deleteFirstNode();
void deleteMiddleNode(int position);

int main()
{
	int n, choice, position;

	printf("Enter the total number of nodes: ");
	scanf("%d", &n);
	createList(n);

	printf("\nData in the list \n");
	displayList();

	printf("\nPress 1 to delete First node, 2 to delete middle node, 3 to delete Last node : ");
	scanf("%d", &choice);

	switch (choice)
	{
	case 1:
		deleteFirstNode();
		break;
	case 2:
		printf("\nEnter the node position you want to delete: ");
		scanf("%d", &position);
		deleteMiddleNode(position);
		break;
	case 3:
		deleteLastNode();
		break;
	}

	printf("\nData in the list \n");
	displayList();

	return 0;
}

void createList(int n)
{
	struct node *newNode, *temp;
	int data, i;

	head = (struct node *)malloc(sizeof(struct node));

	if (head == NULL)
	{
		printf("Unable to allocate memory.");
	}
	else
	{
		printf("Enter the data of node 1: ");
		scanf("%d", &data);

		head->data = data;
		head->next = NULL; 

		temp = head;

		for (i = 2; i <= n; i++)
		{
			newNode = (struct node *)malloc(sizeof(struct node));

			if (newNode == NULL)
			{
				printf("Unable to allocate memory.");
				break;
			}
			else
			{
				printf("Enter the data of node %d: ", i);
				scanf("%d", &data);

				newNode->data = data; 
				newNode->next = NULL; 

				temp->next = newNode; 
				temp = temp->next;
			}
		}

		printf("SINGLY LINKED LIST CREATED SUCCESSFULLY\n");
	}
}

void deleteLastNode()
{
	struct node *toDelete, *secondLastNode;

	if (head == NULL)
	{
		printf("List is already empty.");
	}
	else
	{
		toDelete = head;
		secondLastNode = head;

		while (toDelete->next != NULL)
		{
			secondLastNode = toDelete;
			toDelete = toDelete->next;
		}

		if (toDelete == head)
		{
			head = NULL;
		}
		else
		{
			secondLastNode->next = NULL;
		}
		free(toDelete);

		printf("SUCCESSFULLY DELETED LAST NODE OF LIST\n");
	}
}

void displayList()
{
	struct node *temp;

	if (head == NULL)
	{
		printf("List is empty.");
	}
	else
	{
		temp = head;
		while (temp != NULL)
		{
			printf("Data = %d\n", temp->data); 
			temp = temp->next;        
		}
	}
}

void deleteFirstNode()
{
	struct node *toDelete;

	if (head == NULL)
	{
		printf("List is already empty.");
	}
	else
	{
		toDelete = head;
		head = head->next;

		printf("\nData deleted = %d\n", toDelete->data);

		free(toDelete);

		printf("SUCCESSFULLY DELETED FIRST NODE FROM LIST\n");
	}
}
void deleteMiddleNode(int position)
{
	int i;
	struct node *toDelete, *prevNode;

	if (head == NULL)
	{
		printf("List is already empty.");
	}
	else
	{
		toDelete = head;
		prevNode = head;

		for (i = 2; i <= position; i++)
		{
			prevNode = toDelete;
			toDelete = toDelete->next;

			if (toDelete == NULL)
				break;
		}

		if (toDelete != NULL)
		{
			if (toDelete == head)
				head = head->next;

			prevNode->next = toDelete->next;
			toDelete->next = NULL;
			free(toDelete);
			printf("SUCCESSFULLY DELETED NODE FROM MIDDLE OF LIST\n");
		}
		else
		{
			printf("Invalid position unable to delete.");
		}
	}
}

// 참조링크 : https://codeforwin.org/2015/09/c-program-to-delete-middle-node-of-singly-linked-list.html