import React from 'react'
import { Line } from 'react-chartjs-2'
//timezone

function getFormatDate(date) {
  var month = 1 + date.getMonth()
  var day = date.getDate()
  return month + '/' + day
}

function getBMI(height, weight) {
  return weight / height / height
}

function Chart(body, type, height) {
  var colors = 'rgba(86,115,234,1)'
  //labelArray
  var labelArray = []
  var dataArray = []
  var datefunc

  body.sort(function (a, b) {
    return a.date < b.date ? -1 : a.date > b.date ? 1 : 0
  })
  var i = 1
  if (type === 'kg') {
    for (i = 1; i <= body.length; i++) {
      datefunc = new Date(body[body.length - i].date)
      labelArray.unshift(getFormatDate(datefunc))
      if (body.length > 1 && labelArray[0] === labelArray[1]) {
        labelArray[1] = ''
      }
      dataArray.unshift(body[body.length - i].weight_)
    }
    colors = 'rgba(86,115,234,1)'
  } else if (type === 'bmi') {
    var cmtom = height / 100
    for (i = 1; i <= body.length; i++) {
      datefunc = new Date(body[body.length - i].date)
      labelArray.unshift(getFormatDate(datefunc))
      if (body.length > 1 && labelArray[0] === labelArray[1]) {
        labelArray[1] = ''
      }
      dataArray.unshift(getBMI(cmtom, body[body.length - i].weight_).toFixed(2))
    }
    colors = 'rgba(255,0,0,1)'
  } else {
  }

  //dataArray
  const data = {
    labels: labelArray,

    datasets: [
      {
        label: type,
        fill: false,
        lineTension: 0.1,
        backgroundColor: 'rgba(226,231,251,1)',
        borderColor: colors,
        borderCapStyle: 'round',
        borderDash: [],
        borderDashOffset: 0.0,
        borderJoinStyle: 'miter',
        pointBorderColor: 'rgba(86,115,234,1)',
        pointBackgroundColor: '#fff',
        pointBorderWidth: 1,
        pointHoverRadius: 15,
        pointHoverBackgroundColor: 'rgba(86,115,234,1)',
        pointHoverBorderColor: 'rgba(86,115,234,1)',
        pointHoverBorderWidth: 2,
        pointRadius: 4,
        pointHitRadius: 10,
        data: dataArray,
      },
    ],
  }

  return (
    <Line
      data={data}
      width={440}
      height={200}
      options={{ maintainAspectRatio: true }}
    />
  )
}

export default Chart
