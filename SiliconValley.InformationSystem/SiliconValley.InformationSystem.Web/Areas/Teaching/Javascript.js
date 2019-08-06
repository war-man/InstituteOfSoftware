
layui.use('table', function(){
  var table = layui.table;
  
  table.render({
  elem: '#teacherlist'
  , url:'/Teaching/Teacher/TeacherData/'
  ,cellMinWidth: 80 //全局定义常规单元格的最小宽度，layui 2.2.1 新增
  ,cols: [[
       {field: 'TeacherID', width:80, title: 'ID', sort: true}
      ,{field: 'TeacherName', width:80, title: '姓名'}
      , { field: 'WorkExperience', width: 80, title: '工作经验', sort: true }
      
      ]]
      , page: true
});
});
