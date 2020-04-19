<template>
  <nm-form-dialog ref="form" v-bind="form" :rules="rules" v-on="on" :visible.sync="visible_">
    <el-row>
      <el-col :span="20" :offset="1">
        <el-form-item label="所属模块：" prop="moduleCode">
          <nm-module-select v-model="form.model.moduleCode" />
        </el-form-item>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="10" :offset="1">
        <el-form-item label="任务分组：" prop="group">
          <group-select v-model="form.model.group" />
        </el-form-item>
      </el-col>
      <el-col :span="10">
        <el-form-item label="触发器：" prop="triggerType">
          <el-radio-group v-model="form.model.triggerType" size="small">
            <el-radio-button :label="0">通用</el-radio-button>
            <el-radio-button :label="1">CRON</el-radio-button>
          </el-radio-group>
        </el-form-item>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="10" :offset="1">
        <el-form-item label="任务名称：" prop="name">
          <el-input v-model="form.model.name" />
        </el-form-item>
      </el-col>
      <el-col :span="10">
        <el-form-item label="任务编码：" prop="code">
          <el-input v-model="form.model.code" />
        </el-form-item>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="10" :offset="1">
        <el-form-item label="开始日期：" prop="beginDate">
          <el-date-picker v-model="form.model.beginDate" type="date" placeholder="选择日期" value-format="yyyy-MM-dd"></el-date-picker>
        </el-form-item>
      </el-col>
      <el-col :span="10">
        <el-form-item label="结束日期：" prop="endDate">
          <el-date-picker v-model="form.model.endDate" type="date" placeholder="选择日期" value-format="yyyy-MM-dd"></el-date-picker>
        </el-form-item>
      </el-col>
    </el-row>
    <el-row v-if="form.model.triggerType === 0">
      <el-col :span="10" :offset="1">
        <el-form-item label="间隔时间：" prop="interval">
          <el-input v-model.number="form.model.interval" placeholder="请输入执行时间间隔">
            <template slot="append">秒</template>
          </el-input>
        </el-form-item>
      </el-col>
      <el-col :span="10">
        <el-form-item label="重复次数：" prop="repeatCount">
          <el-input v-model.number="form.model.repeatCount" placeholder="请输入重复次数，0表示无限次">
            <template slot="append">次</template>
          </el-input>
        </el-form-item>
      </el-col>
    </el-row>
    <el-row v-else>
      <el-col :span="20" :offset="1">
        <el-form-item label="CRON表达式：" prop="cron">
          <el-input v-model="form.model.cron" placeholder="请输入CRON表达式"></el-input>
        </el-form-item>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="20" :offset="1">
        <el-form-item label="请求地址：" prop="url">
          <el-input v-model="form.model.url" placeholder="请输入请求地址"></el-input>
        </el-form-item>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="20" :offset="1">
        <el-form-item label="认证方式：" prop="authType">
          <auth-type-select v-model="form.model.authType" />
        </el-form-item>
      </el-col>
    </el-row>
    <el-row v-show="form.model.authType === 1">
      <el-col :span="20" :offset="1">
        <el-form-item label="请求令牌：" prop="token">
          <el-input v-model="form.model.token"></el-input>
        </el-form-item>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="20" :offset="1">
        <el-form-item label="请求方法：" prop="method">
          <el-select v-model="form.model.method" placeholder="请选择">
            <el-option label="GET" :value="0"> </el-option>
            <el-option label="PUT" :value="1"> </el-option>
            <el-option label="POST" :value="2"> </el-option>
            <el-option label="DELETE" :value="3"> </el-option>
          </el-select>
        </el-form-item>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="20" :offset="1">
        <el-form-item label="数据格式：" prop="contentType">
          <content-type-select v-model="form.model.contentType" />
        </el-form-item>
      </el-col>
    </el-row>
    <el-row v-if="form.model.method !== '0'">
      <el-col :span="20" :offset="1">
        <el-form-item label="请求参数：" prop="parameters">
          <el-input type="textarea" v-model="form.model.parameters"></el-input>
        </el-form-item>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="20" :offset="1">
        <el-form-item label="请求头：" prop="headerList">
          <el-row>
            <el-col :span="10">
              <el-input v-model="header.key" placeholder="key:"></el-input>
            </el-col>
            <el-col :span="10" class="nm-p-l-10">
              <el-input v-model="header.value" placeholder="value:"></el-input>
            </el-col>
            <el-col :span="4" class="nm-text-center">
              <nm-button icon="add" @click="onAddHeader" />
            </el-col>
          </el-row>
          <el-table v-show="form.model.headerList.length > 0" :data="form.model.headerList" border size="mini" class="nm-m-t-7" style="width: 100%">
            <el-table-column prop="key" label="键"> </el-table-column>
            <el-table-column prop="value" label="值"> </el-table-column>
            <el-table-column label="删除" width="80">
              <template v-slot="{ row }">
                <nm-button icon="delete" @click="onDelHeader(row)" />
              </template>
            </el-table-column>
          </el-table>
        </el-form-item>
      </el-col>
    </el-row>
  </nm-form-dialog>
</template>
<script>
import { mixins } from 'netmodular-ui'
import GroupSelect from '../../../group/components/select'
import AuthTypeSelect from '../auth-type-select'
import ContentTypeSelect from '../content-type-select'

const api = $api.quartz.job

export default {
  mixins: [mixins.formDialogEdit],
  components: { GroupSelect, AuthTypeSelect, ContentTypeSelect },
  data() {
    return {
      form: {
        header: false,
        labelWidth: '120px',
        width: '900px',
        height: '600px',
        action: api.updateHttpJob,
        model: {
          moduleCode: '',
          group: '',
          name: '',
          code: '',
          triggerType: 0,
          interval: 5,
          repeatCount: 0,
          cron: '',
          beginDate: '',
          endDate: '',
          url: '',
          method: 0,
          parameters: '',
          authType: 0,
          token: '',
          contentType: 0,
          headerList: []
        }
      },
      header: {
        key: '',
        value: ''
      }
    }
  },
  computed: {
    rules() {
      let rules = {
        moduleCode: [{ required: true, message: '请选择模块', trigger: 'change' }],
        group: [{ required: true, message: '请选择任务组', trigger: 'change' }],
        name: [{ required: true, message: '请输入任务名称', trigger: 'blur' }],
        code: [{ required: true, message: '请输入任务编码', trigger: 'blur' }],
        beginDate: [{ required: true, message: '请选择开始日期', trigger: 'change' }],
        endDate: [{ required: true, message: '请选择开始日期', trigger: 'change' }],
        method: [{ required: true, message: '请选择请求方法', trigger: 'change' }],
        authType: [{ required: true, message: '请选择认证方式', trigger: 'change' }],
        contentType: [{ required: true, message: '请选择数据格式', trigger: 'change' }],
        url: [
          { required: true, message: '请输入请求地址', trigger: 'blur' },
          { type: 'url', message: '请输入正确的URL', trigger: 'blur' }
        ]
      }

      if (this.form.model.triggerType === 0) {
        rules['interval'] = [
          { required: true, message: '请输入执行间隔时间', trigger: 'blur' },
          { type: 'integer', min: 1, message: '请输入整数且不能小于1', trigger: 'blur' }
        ]
        rules['repeatCount'] = [
          { required: true, message: '请输入执行次数', trigger: 'blur' },
          { type: 'integer', min: 0, message: '请输入整数且不能小于0', trigger: 'blur' }
        ]
      } else {
        rules['cron'] = [{ required: true, message: '请输入CRON表达式', trigger: 'blur' }]
      }
      if (this.form.model.authType === 1) {
        rules['token'] = [{ required: true, message: '请输入令牌', trigger: 'blur' }]
      }

      return rules
    }
  },
  methods: {
    editAction() {
      return api.editHttpJob(this.id)
    },
    onAddHeader() {
      if (!this.header.key) {
        this._warning('请输入请求头的key')
        return
      }
      if (!this.header.value) {
        this._warning('请输入请求头的value')
        return
      }
      if (this.form.model.headerList.findIndex(m => m.key.toLowerCase() === this.header.key.toLowerCase()) > -1) {
        this._warning('key已存在')
        return
      }

      this.form.model.headerList.push(Object.assign({}, this.header))
      this.header.key = ''
      this.header.value = ''
    },
    onDelHeader(row) {
      let list = this.form.model.headerList
      for (let i = 0; i < list.length; i++) {
        let item = list[i]
        if (item.key.toLowerCase() === row.key.toLowerCase()) {
          list.splice(i, 1)
          return
        }
      }
    }
  }
}
</script>
