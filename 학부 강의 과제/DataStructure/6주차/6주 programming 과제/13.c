#include <stdio.h>
#include <stdlib.h>

struct node {
	int num;
	struct node * preptr;
	struct node * nextptr;
}*stnode, *ennode;


void DlListcreation(int n);
void DlListDeleteFirstNode();
void displayDlList(int a);

int main()
{
	int n, num1, a, insPlc;
	stnode = NULL;
	ennode = NULL;
	printf("\n create doubly list : ");
	scanf("%d", &n);
	DlListcreation(n);
	a = 1;
	displayDlList(a);
	DlListDeleteFirstNode();
	a = 2;
	displayDlList(a);
	return 0;
}

void DlListcreation(int n)
{
	int i, num;
	struct node *fnNode;

	if (n >= 1)
	{
		stnode = (struct node *)malloc(sizeof(struct node));
		if (stnode != NULL)
		{
			printf(" node 1 : "); // assigning data in the first node
			scanf("%d", &num);
			stnode->num = num;
			stnode->preptr = NULL;
			stnode->nextptr = NULL;
			ennode = stnode;
			for (i = 2; i <= n; i++)
			{
				fnNode = (struct node *)malloc(sizeof(struct node));
				if (fnNode != NULL)
				{
					printf(" node %d : ", i);
					scanf("%d", &num);
					fnNode->num = num;
					fnNode->preptr = ennode;    // new node is linking with the previous node
					fnNode->nextptr = NULL;     // set next address of fnnode is NULL
					ennode->nextptr = fnNode;   // previous node is linking with the new node
					ennode = fnNode;            // assign new node as last node
				}
				else
				{
					printf(" Memory can not be allocated.");
					break;
				}
			}
		}
		else
		{
			printf(" Memory can not be allocated.");
		}
	}
}

void DlListDeleteFirstNode()
{
	struct node * NodeToDel;
	if (stnode == NULL)
	{
		printf(" Delete is not possible. No data in the list.\n");
	}
	else
	{
		NodeToDel = stnode;
		stnode = stnode->nextptr;   // move the next address of starting node to 2 node
		stnode->preptr = NULL;      // set previous address of staring node is NULL
		free(NodeToDel);            // delete the first node from memory
	}
}

void displayDlList(int m)
{
	struct node * tmp;
	int n = 1;
	if (stnode == NULL)
	{
		printf(" No data found in the List yet.");
	}
	else
	{
		tmp = stnode;
		if (m == 1)
		{
			printf("\n Before :\n");
		}
		else
		{
			printf("\n After :\n");
		}
		while (tmp != NULL)
		{
			printf("%d ", tmp->num);
			n++;
			tmp = tmp->nextptr; 
		}
	}
}
