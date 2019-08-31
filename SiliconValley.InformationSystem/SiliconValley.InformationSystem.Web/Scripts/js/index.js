 


 
 





//折线图
var line = echarts.init(document.getElementById('line'));
line.setOption({
    color:["#32d2c9"],
    title: {
        x: 'left',
        text: '演讲次数',
        textStyle: {
            fontSize: '18',
            color: '#4c4c4c',
            fontWeight: 'bolder'
        }
    },
            // 提示框
    tooltip: {
        trigger: 'axis'
    },
            //工具框，可以选择
    toolbox: {
        show: true,
        feature: {
            dataZoom: {
                yAxisIndex: 'none'
            },
            dataView: {readOnly: false},
            magicType: {type: ['line', 'bar']}
        }
    },
    xAxis:  {
        type: 'category',
        name:'月份',
       // boundaryGap值为false的时候，折线第一个点在y轴上
        boundaryGap: false,
        data: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月' ] , 
        axisLabel: {
            interval:0
        }
    },
    yAxis: {
        name:'次数',
        type: 'value',// 设置y轴刻度的最小值
        min: 0,
        max:200
    },
    series: [
        {
            name:'2018',
            type: 'line',
            // 设置拐点为实心圆
            symbol: 'circle', 
            data:[30, 50, 70, 90, 110, 130,79,45,89,94,60,100],
            markLine: { data: [{ type: 'average', name: '平均值' }] }//平均值
        },
        {
            name: '2019',
            type: 'line',
            // 设置拐点为实心圆
            symbol: 'circle',            
            data: [40, 50, 40, 80, 90, 80, 60, 99, 30, 94, 35, 53],
            markLine: { data: [{ type: 'average', name: '平均值' }] },//平均值             
            // smooth: 0.3//曲线弧度
        }
    ],
    color: ['blue', 'red']
}) ;

 
