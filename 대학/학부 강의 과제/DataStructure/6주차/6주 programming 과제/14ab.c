#include <stdio.h>
#include <string.h>

void Remove_LastOccurrence(char *str, char ch);

int main()
{
	char str[100], ch;

	printf("\n Please Enter any String :  ");
	gets(str);

	printf("\n Please Enter the Character that you want to Search for :  ");
	scanf("%c", &ch);

	Remove_LastOccurrence(str, ch);

	printf("\n The Final String after Removing First Occurrence of '%c' = %s ", ch, str);
	return 0;
}

void Remove_LastOccurrence(char *str, char ch)
{
	int i, index, len;

	len = strlen(str);

	for (i = 0; i < len; i++)
	{
		if (str[i] == ch)
		{
			index = i;
		}
	}

	if (index != -1)
	{
		i = index;

		while (i < len)
		{
			str[i] = str[i + 1];
			i++;
		}
	}
}