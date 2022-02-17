
from bs4 import BeautifulSoup
from datetime import datetime
import requests

# Import configured variables
from DUTAPI.__Variables__ import *
from DUTAPI.Enums import *

def GetNews(type: NewsType = NewsType.General, page: int = 1):
    """
    Get news from sv.dut.udn.vn.
    For latest news, leave 'page' to default.

    type (NewsType): Type of news want to load.
    page (int): News page. If less than 1, will be reset to 1.
    """

    # Prepare a result data    
    jsonReturn = {}
    jsonReturn['date'] = datetime.today().strftime("%Y/%m/%d")
    jsonReturn['time'] = datetime.today().strftime("%H:%M:%S")
    jsonReturn['newstype'] = type.value
    jsonReturn['newslist'] = []

    if (page < 1):
        page = 1

    try:
        # Get elements from sv.dut.dut.vn
        if (type == NewsType.General):
            webHTML = requests.get(URL_NEWSGENERAL.format(page = page))
        else:
            webHTML = requests.get(URL_NEWSSUBJECTS.format(page = page))
        
        # Convert to BeautifulSoup
        soup = BeautifulSoup(webHTML.content, 'lxml')
        # Find all element groups in html.
        news = soup.findAll('div', {'class': 'tbBox'})

        for i in range(0, len(news), 1):
            # Get news date and title
            webElement = news[i].find('div', class_='tbBoxCaption')
            date = webElement.find_all('span')[0].text
            title = webElement.find_all('span')[1].text
            # Get news content
            content = news[i].find('div', class_='tbBoxContent').text
            # Add to jsonReturn
            jsonReturn['newslist'].append({
                'date': date,
                'title': title,
                'contenttext': content
            })
    except:
        # If something went wrong, delete all items in news list.
        jsonReturn['newslist'].clear()
    finally:
        # Return result
        return jsonReturn
