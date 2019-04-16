db.createCollection("TrackLog")
//db.TrackLog.dropIndex({RequestTime:1})
//db.TrackLog.createIndex({RequestTime:1},{expireAfterSeconds:60*60*24*1})
db.TrackLog.ensureIndex({"RequestTime":1},{expireAfterSeconds:60*60*24*1},{"background":true})
//db.TrackLog.getIndexes()
//db.TrackLog.find({Url:"testurl003"}).explain()
//db.TrackLog.remove({ActionName:"Post"})
//固定集合 KB
//db.createCollection("TrackLog",{capped:true,size:102400,max:100000}); 

db.createCollection("AppLog")
db.AppLog.ensureIndex({Message:"text"})