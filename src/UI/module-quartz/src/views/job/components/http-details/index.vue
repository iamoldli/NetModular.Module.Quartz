<template>
  <nm-drawer v-bind="drawer" :visible.sync="visible_" @open="onOpen">
    <nm-details ref="details" v-bind="details"></nm-details>
  </nm-drawer>
</template>
<script>
import { mixins } from 'netmodular-ui'
const api = $api.quartz.job
export default {
  mixins: [mixins.dialog],
  data() {
    return {
      drawer: {
        header: true,
        title: 'HTTP任务详情',
        icon: 'web',
        noPadding: true
      },
      details: {
        labelWidth: '100px',
        action: this.query,
        options: [
          [
            {
              label: '请求地址',
              prop: 'url'
            }
          ],
          [
            {
              label: '请求方法',
              prop: 'methodName'
            }
          ],
          [
            {
              label: '认证方式',
              prop: 'authTypeName'
            }
          ],
          [
            {
              label: '令牌',
              prop: 'token'
            }
          ],
          [
            {
              label: '数据格式',
              prop: 'ContentTypeName'
            }
          ],
          [
            {
              label: '请求参数',
              prop: 'parameters'
            }
          ],
          [
            {
              label: '请求头',
              prop: 'headers'
            }
          ]
        ]
      }
    }
  },
  props: {
    id: String
  },
  methods: {
    query() {
      if (this.id) return api.jobHttpDetails(this.id)
      else
        return new Promise(resolve => {
          resolve()
        })
    },
    onOpen() {
      this.$refs.details.refresh()
    }
  }
}
</script>
