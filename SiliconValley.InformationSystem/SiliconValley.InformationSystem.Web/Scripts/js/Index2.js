//柱状图
var pillar1 = echarts.init(document.getElementById('pillar1'));
pillar1.setOption({
    color: ["#ce6e73"],
    title: {
        subtext: '数量'
    },
    tooltip: {
        trigger: 'axis'
    },
    legend: {
        x: 'right',
        data: ['上门量']
    },
    calculable: true,
    xAxis: [
        {
            type: 'category',
            data: ['一月', '二月', '三月', '四月', '五月', '六月',
                '七月', '八月', '九月', '十月', '十一月', '十二月']
        }
    ],
    yAxis: [
        {
            type: 'value',
            min: 0,
            max:1000
        }
    ],
    series: [
        {
            name: '上门量',
            type: 'bar',
            data: [74, 62, 56, 79, 80, 30, 55, 35, 38, 41, 75, 89]
        } 
    ]
});