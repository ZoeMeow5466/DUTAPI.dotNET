
from bs4 import BeautifulSoup
from datetime import datetime
import requests

# Import configured variables
from DUTAPI.__Variables__ import *
from DUTAPI.Enums import *

class Session:

    def __init__(self):
        self.WEB_SESSION = requests.Session()
        return

    def IsLoggedIn(self):
        """
        Check if your account is logged in from sv.dut.udn.vn.
        """

        # Prepare a result data.
        result = {}
        result['date'] = datetime.today().strftime("%Y/%m/%d")
        result['time'] = datetime.today().strftime("%H:%M:%S")
        result['loggedin'] = False
        try:
            if ((self.WEB_SESSION.get(URL_ACCOUNTCHECKLOGIN).status_code) in [200, 204]):
                result['loggedin'] = True
        except:
            # If something went wrong, 'loggedin' will False.
            result['loggedin'] = False
        finally:
            # Return result
            return result


    def Login(self, username: str, password: str):
        """
        Login to sv.dut.udn.vn using your account provided by DUT school.
        
        username (string): Username (i.e. Student ID).
        password (string): Password
        """

        # Prepare a result data.
        result = {}
        result['date'] = datetime.today().strftime("%Y/%m/%d")
        result['time'] = datetime.today().strftime("%H:%M:%S")
        result['loggedin'] = False
        # 
        dataRequest = {}
        dataRequest['__VIEWSTATE'] = VIEWSTATE
        dataRequest['__VIEWSTATEGENERATOR'] = '20CC0D2F'
        dataRequest['_ctl0:MainContent:DN_txtAcc'] = username
        dataRequest['_ctl0:MainContent:DN_txtPass'] = password
        dataRequest['_ctl0:MainContent:QLTH_btnLogin'] = 'Đăng+nhập'
        self.WEB_SESSION.post(URL_ACCOUNTLOGIN, data=dataRequest)
        #
        try:
            if (self.IsLoggedIn()['loggedin']):
                result['loggedin'] = True
        except:
            # If something went wrong, 'loggedin' will False.
            result['loggedin'] = False
        finally:
            # Return result
            return result

            
    def Logout(self):
        """
        Logout your account from sv.dut.udn.vn.
        """

        # Prepare a result data.
        result = {}
        result['date'] = datetime.today().strftime("%Y/%m/%d")
        result['time'] = datetime.today().strftime("%H:%M:%S")
        result['loggedout'] = False
        try:
            self.WEB_SESSION.get(URL_ACCOUNTLOGOUT)
            if (self.IsLoggedIn() != True):
                result['loggedout'] = True
        except:
            result['loggedout'] = True
        finally:
            return result

    def __TableRowToJson__(self, row, dataInput):
        result = {}

        try:
            cell = row.find_all('td', {'class':'GridCell'})
            for i in range(0, len(dataInput), 1):
                if (dataInput[i][2].lower() == 'num'):
                    try:
                        result[dataInput[i][0]] = float(cell[dataInput[i][1]].text.replace(',',''))
                    except:
                        result[dataInput[i][0]] = 0
                elif (dataInput[i][2].lower() == 'bool'):
                    if 'GridCheck' in cell[dataInput[i][1]].attrs.get('class'):
                        result[dataInput[i][0]] = True
                    else:
                        result[dataInput[i][0]] = False
                elif (dataInput[i][2].lower() == 'string'):
                    result[dataInput[i][0]] = cell[dataInput[i][1]].text
                else:
                    pass
            pass
        except:
            result = {}
        finally:
            return result

    def GetSubjectSchedule(self, year: int = 20, semester: int = 1, studyAtSummer: bool = False):
        """
        Get all subject schedule (study and examination) from a year you choosed.

        year (int): 2-digit year.
        semester (int): 1 or 2
        studyAtSummer (bool): Show schedule if you has studied in summer. 'semester' must be 2, otherwise will getting exception.
        """

        result = {}
        result['date'] = datetime.today().strftime("%Y/%m/%d")
        result['time'] = datetime.today().strftime("%H:%M:%S")
        result['totalcredit'] = 0.0
        result['schedulelist'] = []

        try:
            if studyAtSummer:
                satS = 1
            else:
                satS = 0
            url = URL_ACCOUNTSCHEDULE.format(nam = year, hocky = semester, hoche = satS)
            webHTML = self.WEB_SESSION.get(url)
            soup = BeautifulSoup(webHTML.content, 'lxml')
            
            # Find all subjects schedule when study
            schStudyTable = soup.find('table', {'id': 'TTKB_GridInfo'})
            schStudyRow = schStudyTable.find_all('tr', {'class': 'GridRow'})

            dataSchStudy = [
                ['ID', 1, 'string'],
                ['Name', 2, 'string'],
                ['Credit', 3, 'num'],
                ['IsHighQuality', 5, 'bool'],
                ['Lecturer', 6, 'string'],
                ['ScheduleStudy', 7, 'string'],
                ['Weeks', 8, 'string'],
                ['PointFomula', 10, 'string']
            ]

            for i in range(0, len(schStudyRow) - 1, 1):
                resultRow = self.__TableRowToJson__(schStudyRow[i], dataSchStudy)
                result['totalcredit'] += resultRow['Credit']
                result['schedulelist'].append(resultRow)

            # Find all subjects schedule examination
            schExamTable = soup.find('table', {'id': 'TTKB_GridLT'})
            schExamRow = schExamTable.find_all('tr', {'class': 'GridRow'})

            dataSchExam = [
                ['ID', 1, 'string'],
                ['Name', 2, 'string'],
                ['GroupExam', 3, 'string'],
                ['IsGlobalExam', 4, 'bool'],
                ['DateExamInString', 5, 'string']
            ]

            for i in range(0, len(schExamRow), 1):
                resultRow = self.__TableRowToJson__(schExamRow[i], dataSchExam)
                for j in range (0, len(result['schedulelist']), 1):
                    if result['schedulelist'][i]['Name'] == resultRow['Name']:
                        result['schedulelist'][i]['GroupExam'] = resultRow['GroupExam']
                        result['schedulelist'][i]['IsGlobalExam'] = resultRow['IsGlobalExam']
                        result['schedulelist'][i]['DateExamInString'] = resultRow['DateExamInString']
        except:
            result['totalcredit'] = 0.0
            result['schedulelist'].clear()
        finally:
            return result

    def GetSubjectFee(self, year: int = 20, semester: int = 1, studyAtSummer: bool = False):
        """
        Get all subject fee from a year you choosed.

        year (int): 2-digit year.
        semester (int): 1 or 2
        studyAtSummer (bool): Show schedule if you has studied in summer. 'semester' must be 2, otherwise will getting exception.
        """

        result = {}
        result['date'] = datetime.today().strftime("%Y/%m/%d")
        result['time'] = datetime.today().strftime("%H:%M:%S")
        result['totalcredit'] = 0
        result['totalmoney'] = 0
        result['feelist'] = []
        
        try:
            if studyAtSummer:
                satS = 1
            else:
                satS = 0
            webHTML = self.WEB_SESSION.get(URL_ACCOUNTFEE.format(nam = year, hocky = semester, hoche = satS))
            soup = BeautifulSoup(webHTML.content, 'lxml')
            feeTable = soup.find('table', {'id': 'THocPhi_GridInfo'})
            feeRow = feeTable.find_all('tr', {'class': 'GridRow'})

            dataInput = [
                ['ID', 1, 'string'],
                ['Name', 2, 'string'],
                ['Credit', 3, 'num'],
                ['IsHighQuality', 4, 'bool'],
                ['Price', 5, 'num'],
                ['Debt', 6, 'bool'],
                ['IsReStudy', 7, 'bool'],
                ['VerifiedPaymentAt', 8, 'string']
            ]

            for i in range(0, len(feeRow) - 1, 1):
                resultRow = self.__TableRowToJson__(feeRow[i], dataInput)
                result['totalcredit'] += resultRow['Credit']
                result['totalmoney'] += resultRow['Price']
                result['feelist'].append(resultRow)
        except:
            result['totalcredit'] = 0
            result['totalmoney'] = 0
            result['feelist'] = []
        finally:
            return result
