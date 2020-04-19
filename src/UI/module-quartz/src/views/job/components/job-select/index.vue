<script>
import { mixins } from 'netmodular-ui'

const api = $api.quartz.job

export default {
  mixins: [mixins.select],
  data() {
    return {
      action: this.query
    }
  },
  props: {
    moduleCode: {
      type: String
    }
  },
  methods: {
    query() {
      if (this.moduleCode) {
        return api.jobSelect(this.moduleCode)
      } else {
        return new Promise(resolve => {
          resolve([])
        })
      }
    }
  },
  watch: {
    moduleCode(newVal, oldVal) {
      if (oldVal) this.reset()
      this.refresh()
    }
  }
}
</script>
