#include <stdio.h>
#include <stdlib.h>

struct Node
{
	int data;
	struct Node* next;
};

void printList(struct Node* head)
{
	struct Node* ptr = head;
	while (ptr)
	{
		printf("%d -> ", ptr->data);
		ptr = ptr->next;
	}

	printf("null");
}

void push(struct Node** head, int data)
{
	struct Node* newNode = (struct Node*)malloc(sizeof(struct Node));
	newNode->data = data;
	newNode->next = *head;
	*head = newNode;
}

struct Node* CopyList(struct Node* head)
{
	struct Node* current = head;
	struct Node* newList = NULL;
	struct Node* tail = NULL;	

	while (current != NULL)
	{
		if (newList == NULL)
		{
			newList = (struct Node*)malloc(sizeof(struct Node));
			newList->data = current->data;
			newList->next = NULL;
			tail = newList;
		}
		else
		{
			tail->next = (struct Node*)malloc(sizeof(struct Node));
			tail = tail->next;
			tail->data = current->data;
			tail->next = NULL;
		}
		current = current->next;
	}
	return newList;
}

int main(void)
{
	int keys[] = { 1, 2, 3, 4 };
	int n = sizeof(keys) / sizeof(keys[0]);
	struct Node* head = NULL;
	for (int i = n - 1; i >= 0; i--)
		push(&head, keys[i]);

	struct Node* dup = CopyList(head);

	printList(dup);

	return 0;
}