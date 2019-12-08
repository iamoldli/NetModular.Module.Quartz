<template>
  <nm-container>
    <nm-list ref="list" v-bind="list">
      <!--查询条件-->
      <template v-slot:querybar>
        <el-form-item label="名称：" prop="name">
          <el-input v-model="list.model.name" clearable />
        </el-form-item>
      </template>

      <!--按钮-->
      <template v-slot:querybar-buttons>
        <nm-button v-bind="buttons.add" @click="add" />
      </template>

      <template v-slot:col-jobClass="{ row }">{{ row.jobClass.split(',')[0] }}</template>
      <template v-slot:col-typeName="{ row }">
        <el-tooltip v-if="row.jobType === 1" effect="dark" content="点击查看HTTP详情" placement="top">
          <nm-button type="text" :text="row.typeName" @click="httpDetails(row)" />
        </el-tooltip>
        <span v-else>{{ row.typeName }}</span>
      </template>

      <template v-slot:col-triggerType="{ row }">
        <el-tag v-if="row.triggerType === 0" size="small">通用</el-tag>
        <el-tag v-else type="warning" size="small">CRON</el-tag>
      </template>

      <template v-slot:col-interval="{ row }">
        <span v-if="row.triggerType === 0">{{ row.interval }}秒</span>
        <span v-else>-</span>
      </template>

      <template v-slot:col-repeatCount="{ row }">
        <span v-if="row.triggerType === 1">-</span>
        <span v-else-if="row.repeatCount === 0">无限次</span>
        <span v-else>{{ row.repeatCount }}次</span>
      </template>

      <template v-slot:col-cron="{ row }">
        <span v-if="row.triggerType === 0">-</span>
        <span v-else>{{ row.cron }}</span>
      </template>

      <template v-slot:col-beginDate="{ row }"> {{ $dayjs(row.beginDate).format('YYYY-MM-DD') }} ~ {{ $dayjs(row.endDate).format('YYYY-MM-DD') }} </template>

      <template v-slot:col-status="{ row }">
        <el-tag v-if="row.status === 0" type="info" effect="dark" size="small">停止</el-tag>
        <el-tag v-else-if="row.status === 1" type="success" effect="dark" size="small">运行中</el-tag>
        <el-tag v-else-if="row.status === 2" type="warning" effect="dark" size="small">暂停</el-tag>
        <el-tag v-else-if="row.status === 3" type="primary" effect="dark" size="small">已完成</el-tag>
        <el-tag v-else type="danger" effect="dark" size="small">异常</el-tag>
      </template>

      <!--操作列-->
      <template v-slot:col-operation="{ row }">
        <nm-button v-bind="buttons.pause" v-if="row.status === 1" @click="pause(row)" />
        <nm-button v-bind="buttons.resume" v-if="[0, 2, 3].includes(row.status)" @click="resume(row)" />
        <nm-button v-bind="buttons.stop" v-if="[1, 2].includes(row.status)" @click="stop(row)" />
        <nm-button v-bind="buttons.log" @click="log(row)" />
        <nm-button v-bind="buttons.edit" @click="edit(row)" />
        <nm-button-delete v-bind="buttons.del" :id="row.id" :action="removeAction" @success="refresh" />
      </template>
    </nm-list>

    <!--添加-->
    <add-page :visible.sync="dialog.add" @success="refresh" />
    <!--编辑-->
    <edit-normal :id="curr.id" :visible.sync="dialog.edit" @success="refresh" />
    <!--编辑HTTP-->
    <edit-http :id="curr.id" :visible.sync="dialog.editHttp" @success="refresh" />
    <!--日志-->
    <job-log :id="curr.id" :name="curr.name" :visible.sync="dialog.log" />
    <!--HTTP任务详情-->
    <http-details :id="curr.id" :visible.sync="dialog.httpDetails" />
  </nm-container>
</template>
<script>
import page from './page'
import cols from './cols'
import AddPage from '../components/add'
import EditNormal from '../components/edit-normal'
import EditHttp from '../components/edit-http'
import JobLog from '../components/log-list'
import HttpDetails from '../components/http-details'

const api = $api.quartz.job

export default {
  name: page.name,
  components: { AddPage, EditNormal, EditHttp, JobLog, HttpDetails },
  data() {
    return {
      curr: { id: '', name: '' },
      list: {
        title: page.title,
        cols,
        operationWidth: '320',
        action: api.query,
        model: {
          name: ''
        }
      },
      removeAction: api.remove,
      dialog: {
        add: false,
        edit: false,
        editHttp: false,
        log: false,
        httpDetails: false
      },
      buttons: page.buttons
    }
  },
  methods: {
    refresh() {
      this.$refs.list.refresh()
    },
    add() {
      this.dialog.add = true
    },
    edit(row) {
      this.curr = row
      if (row.jobType === 0) this.dialog.edit = true
      else this.dialog.editHttp = true
    },
    pause(row) {
      this._confirm(`您确定要暂停《${row.name}》任务吗?`, '提醒').then(() => {
        this._openLoading(`正在暂停任务《${row.name}》，请稍后...`)
        api
          .pause(row.id)
          .then(() => {
            row.status = 2
            this._closeLoading()
          })
          .catch(() => {
            this._closeLoading()
          })
      })
    },
    resume(row) {
      this._confirm(`您确定要启动《${row.name}》任务吗?`, '提醒').then(() => {
        this._openLoading(`正在启动任务《${row.name}》，请稍后...`)
        api
          .resume(row.id)
          .then(() => {
            row.status = 1
            this._closeLoading()
          })
          .catch(() => {
            this._closeLoading()
          })
      })
    },
    stop(row) {
      this._confirm(`您确定要停止《${row.name}》任务吗?`, '警告', 'danger').then(() => {
        this._openLoading(`正在停止任务《${row.name}》，请稍后...`)
        api
          .stop(row.id)
          .then(() => {
            row.status = 0
            this._closeLoading()
          })
          .catch(() => {
            this._closeLoading()
          })
      })
    },
    log(row) {
      this.curr = row
      this.dialog.log = true
    },

    httpDetails(row) {
      this.curr = row
      this.dialog.httpDetails = true
    }
  }
}
</script>
