
import DUTAPI
import json

# print(json.dumps(DUTAPI.GetNews(DUTAPI.NewsType.General, 1), ensure_ascii=False, indent=3))
dutSS = DUTAPI.Session()
print(dutSS.Login('102190147', 'cloney1301'))
print(dutSS.IsLoggedIn())
# print(json.dumps(dutSS.GetSubjectSchedule(21, 2, False), ensure_ascii=False, indent=3))
print(json.dumps(dutSS.GetSubjectFee(21, 2, False), ensure_ascii=False, indent=3))
print(dutSS.Logout())
print(dutSS.IsLoggedIn())
